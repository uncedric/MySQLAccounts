using System.Collections.Generic;
using System.Security.Claims;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that represents the UserClaims table in the MySQL Database
    public class UserClaimsTable
    {
        private MySQLDatabase _database;

        //
        // Summary:
        //     Constructor that takes a MySQLDatabase instance
        //
        // Parameters:
        //   database:
        public UserClaimsTable(MySQLDatabase database)
        {
            _database = database;
        }

        //
        // Summary:
        //     Returns a ClaimsIdentity instance given a userId
        //
        // Parameters:
        //   userId:
        //     The user's id
        public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            string commandText = "Select * from UserClaims where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@UserId", userId } };
            List<Dictionary<string, string>> list = _database.Query(commandText, parameters);
            foreach (Dictionary<string, string> item in list)
            {
                Claim claim = new Claim(item["ClaimType"], item["ClaimValue"]);
                claimsIdentity.AddClaim(claim);
            }

            return claimsIdentity;
        }

        //
        // Summary:
        //     Deletes all claims from a user given a userId
        //
        // Parameters:
        //   userId:
        //     The user's id
        public int Delete(string userId)
        {
            string commandText = "Delete from UserClaims where UserId = @userId";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("userId", userId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Inserts a new claim in UserClaims table
        //
        // Parameters:
        //   userClaim:
        //     User's claim to be added
        //
        //   userId:
        //     User's id
        public int Insert(Claim userClaim, string userId)
        {
            string commandText = "Insert into UserClaims (ClaimValue, ClaimType, UserId) values (@value, @type, @userId)";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("value", userClaim.Value);
            dictionary.Add("type", userClaim.Type);
            dictionary.Add("userId", userId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Deletes a claim from a user
        //
        // Parameters:
        //   user:
        //     The user to have a claim deleted
        //
        //   claim:
        //     A claim to be deleted from user
        public int Delete(IdentityUser user, Claim claim)
        {
            string commandText = "Delete from UserClaims where UserId = @userId and @ClaimValue = @value and ClaimType = @type";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("userId", user.Id);
            dictionary.Add("value", claim.Value);
            dictionary.Add("type", claim.Type);
            return _database.Execute(commandText, dictionary);
        }
    }
}