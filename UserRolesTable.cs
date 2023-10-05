using System.Collections.Generic;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that represents the UserRoles table in the MySQL Database
    public class UserRolesTable
    {
        private MySQLDatabase _database;

        //
        // Summary:
        //     Constructor that takes a MySQLDatabase instance
        //
        // Parameters:
        //   database:
        public UserRolesTable(MySQLDatabase database)
        {
            _database = database;
        }

        //
        // Summary:
        //     Returns a list of user's roles
        //
        // Parameters:
        //   userId:
        //     The user's id
        public List<string> FindByUserId(string userId)
        {
            List<string> list = new List<string>();
            string commandText = "Select Roles.Name from UserRoles, Roles where UserRoles.UserId = @userId and UserRoles.RoleId = Roles.Id";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@userId", userId);
            List<Dictionary<string, string>> list2 = _database.Query(commandText, dictionary);
            foreach (Dictionary<string, string> item in list2)
            {
                list.Add(item["Name"]);
            }

            return list;
        }

        //
        // Summary:
        //     Deletes all roles from a user in the UserRoles table
        //
        // Parameters:
        //   userId:
        //     The user's id
        public int Delete(string userId)
        {
            string commandText = "Delete from UserRoles where UserId = @userId";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("UserId", userId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Inserts a new role for a user in the UserRoles table
        //
        // Parameters:
        //   user:
        //     The User
        //
        //   roleId:
        //     The Role's id
        public int Insert(IdentityUser user, string roleId)
        {
            string commandText = "Insert into UserRoles (UserId, RoleId) values (@userId, @roleId)";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("userId", user.Id);
            dictionary.Add("roleId", roleId);
            return _database.Execute(commandText, dictionary);
        }
    }
}