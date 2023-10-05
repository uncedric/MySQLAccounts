

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that encapsulates a MySQL database connections and CRUD operations
    public class MySQLDatabase : IDisposable
    {
        private MySqlConnection _connection;

        public MySQLDatabase()
            : this("DefaultConnection")
        {
        }

        //
        // Summary:
        //     Constructor which takes the connection string name
        //
        // Parameters:
        //   connectionStringName:
        public MySQLDatabase(string connectionStringName)
        {
            string connectionString = ConfigurationManager.get_ConnectionStrings().get_Item(connectionStringName).get_ConnectionString();
            _connection = new MySqlConnection(connectionString);
        }

        //
        // Summary:
        //     Executes a non-query MySQL statement
        //
        // Parameters:
        //   commandText:
        //     The MySQL query to execute
        //
        //   parameters:
        //     Optional parameters to pass to the query
        //
        // Returns:
        //     The count of records affected by the MySQL statement
        public int Execute(string commandText, Dictionary<string, object> parameters)
        {
            int num = 0;
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                MySqlCommand mySqlCommand = CreateCommand(commandText, parameters);
                return mySqlCommand.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }

        //
        // Summary:
        //     Executes a MySQL query that returns a single scalar value as the result.
        //
        // Parameters:
        //   commandText:
        //     The MySQL query to execute
        //
        //   parameters:
        //     Optional parameters to pass to the query
        public object QueryValue(string commandText, Dictionary<string, object> parameters)
        {
            object obj = null;
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                MySqlCommand mySqlCommand = CreateCommand(commandText, parameters);
                return mySqlCommand.ExecuteScalar();
            }
            finally
            {
                EnsureConnectionClosed();
            }
        }

        //
        // Summary:
        //     Executes a SQL query that returns a list of rows as the result.
        //
        // Parameters:
        //   commandText:
        //     The MySQL query to execute
        //
        //   parameters:
        //     Parameters to pass to the MySQL query
        //
        // Returns:
        //     A list of a Dictionary of Key, values pairs representing the ColumnName and corresponding
        //     value
        public List<Dictionary<string, string>> Query(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, string>> list = null;
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                MySqlCommand mySqlCommand = CreateCommand(commandText, parameters);
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                list = new List<Dictionary<string, string>>();
                while (mySqlDataReader.Read())
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    for (int i = 0; i < mySqlDataReader.FieldCount; i++)
                    {
                        string name = mySqlDataReader.GetName(i);
                        string value = (mySqlDataReader.IsDBNull(i) ? null : mySqlDataReader.GetString(i));
                        dictionary.Add(name, value);
                    }

                    list.Add(dictionary);
                }

                return list;
            }
            finally
            {
                EnsureConnectionClosed();
            }
        }

        //
        // Summary:
        //     Opens a connection if not open
        private void EnsureConnectionOpen()
        {
            int num = 3;
            if (_connection.State != ConnectionState.Open)
            {
                while (num >= 0 && _connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                    num--;
                    Thread.Sleep(30);
                }
            }
        }

        //
        // Summary:
        //     Closes a connection if open
        public void EnsureConnectionClosed()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        //
        // Summary:
        //     Creates a MySQLCommand with the given parameters
        //
        // Parameters:
        //   commandText:
        //     The MySQL query to execute
        //
        //   parameters:
        //     Parameters to pass to the MySQL query
        private MySqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
        {
            MySqlCommand mySqlCommand = _connection.CreateCommand();
            mySqlCommand.CommandText = commandText;
            AddParameters(mySqlCommand, parameters);
            return mySqlCommand;
        }

        //
        // Summary:
        //     Adds the parameters to a MySQL command
        //
        // Parameters:
        //   commandText:
        //     The MySQL query to execute
        //
        //   parameters:
        //     Parameters to pass to the MySQL query
        private static void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                MySqlParameter mySqlParameter = command.CreateParameter();
                mySqlParameter.ParameterName = parameter.Key;
                mySqlParameter.Value = parameter.Value ?? DBNull.Value;
                command.Parameters.Add(mySqlParameter);
            }
        }

        //
        // Summary:
        //     Helper method to return query a string value
        //
        // Parameters:
        //   commandText:
        //     The MySQL query to execute
        //
        //   parameters:
        //     Parameters to pass to the MySQL query
        //
        // Returns:
        //     The string value resulting from the query
        public string GetStrValue(string commandText, Dictionary<string, object> parameters)
        {
            return QueryValue(commandText, parameters) as string;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
