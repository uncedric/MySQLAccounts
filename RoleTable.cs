

using System;
using System.Collections.Generic;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that represents the Role table in the MySQL Database
    public class RoleTable
    {
        private MySQLDatabase _database;

        //
        // Summary:
        //     Constructor that takes a MySQLDatabase instance
        //
        // Parameters:
        //   database:
        public RoleTable(MySQLDatabase database)
        {
            _database = database;
        }

        //
        // Summary:
        //     Deltes a role from the Roles table
        //
        // Parameters:
        //   roleId:
        //     The role Id
        public int Delete(string roleId)
        {
            string commandText = "Delete from Roles where Id = @id";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@id", roleId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Inserts a new Role in the Roles table
        //
        // Parameters:
        //   roleName:
        //     The role's name
        public int Insert(IdentityRole role)
        {
            string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@name", role.Name);
            dictionary.Add("@id", role.Id);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Returns a role name given the roleId
        //
        // Parameters:
        //   roleId:
        //     The role Id
        //
        // Returns:
        //     Role name
        public string GetRoleName(string roleId)
        {
            string commandText = "Select Name from Roles where Id = @id";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@id", roleId);
            return _database.GetStrValue(commandText, dictionary);
        }

        //
        // Summary:
        //     Returns the role Id given a role name
        //
        // Parameters:
        //   roleName:
        //     Role's name
        //
        // Returns:
        //     Role's Id
        public string GetRoleId(string roleName)
        {
            string result = null;
            string commandText = "Select Id from Roles where Name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", roleName } };
            object obj = _database.QueryValue(commandText, parameters);
            if (obj != null)
            {
                return Convert.ToString(obj);
            }

            return result;
        }

        //
        // Summary:
        //     Gets the IdentityRole given the role Id
        //
        // Parameters:
        //   roleId:
        public IdentityRole GetRoleById(string roleId)
        {
            string roleName = GetRoleName(roleId);
            IdentityRole result = null;
            if (roleName != null)
            {
                result = new IdentityRole(roleName, roleId);
            }

            return result;
        }

        //
        // Summary:
        //     Gets the IdentityRole given the role name
        //
        // Parameters:
        //   roleName:
        public IdentityRole GetRoleByName(string roleName)
        {
            string roleId = GetRoleId(roleName);
            IdentityRole result = null;
            if (roleId != null)
            {
                result = new IdentityRole(roleName, roleId);
            }

            return result;
        }

        public int Update(IdentityRole role)
        {
            string commandText = "Update Roles set Name = @name where Id = @id";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@id", role.Id);
            return _database.Execute(commandText, dictionary);
        }
    }
}
