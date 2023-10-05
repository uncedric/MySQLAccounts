using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that represents the UserLogins table in the MySQL Database
    public class UserLoginsTable
    {
        private MySQLDatabase _database;

        //
        // Summary:
        //     Constructor that takes a MySQLDatabase instance
        //
        // Parameters:
        //   database:
        public UserLoginsTable(MySQLDatabase database)
        {
            _database = database;
        }

        //
        // Summary:
        //     Deletes a login from a user in the UserLogins table
        //
        // Parameters:
        //   user:
        //     User to have login deleted
        //
        //   login:
        //     Login to be deleted from user
        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("UserId", user.Id);
            dictionary.Add("loginProvider", login.LoginProvider);
            dictionary.Add("providerKey", login.ProviderKey);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Deletes all Logins from a user in the UserLogins table
        //
        // Parameters:
        //   userId:
        //     The user's id
        public int Delete(string userId)
        {
            string commandText = "Delete from UserLogins where UserId = @userId";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("UserId", userId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Inserts a new login in the UserLogins table
        //
        // Parameters:
        //   user:
        //     User to have new login added
        //
        //   login:
        //     Login to be added
        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("loginProvider", login.LoginProvider);
            dictionary.Add("providerKey", login.ProviderKey);
            dictionary.Add("userId", user.Id);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Return a userId given a user's login
        //
        // Parameters:
        //   userLogin:
        //     The user's login info
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("loginProvider", userLogin.LoginProvider);
            dictionary.Add("providerKey", userLogin.ProviderKey);
            return _database.GetStrValue(commandText, dictionary);
        }

        //
        // Summary:
        //     Returns a list of user's logins
        //
        // Parameters:
        //   userId:
        //     The user's id
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> list = new List<UserLoginInfo>();
            string commandText = "Select * from UserLogins where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@userId", userId } };
            List<Dictionary<string, string>> list2 = _database.Query(commandText, parameters);
            foreach (Dictionary<string, string> item2 in list2)
            {
                UserLoginInfo item = new UserLoginInfo(item2["LoginProvider"], item2["ProviderKey"]);
                list.Add(item);
            }

            return list;
        }
    }
}