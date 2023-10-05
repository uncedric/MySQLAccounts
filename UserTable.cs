using System;
using System.Collections.Generic;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that represents the Users table in the MySQL Database
    public class UserTable<TUser> where TUser : IdentityUser
    {
        private MySQLDatabase _database;

        //
        // Summary:
        //     Constructor that takes a MySQLDatabase instance
        //
        // Parameters:
        //   database:
        public UserTable(MySQLDatabase database)
        {
            _database = database;
        }

        //
        // Summary:
        //     Returns the user's name given a user id
        //
        // Parameters:
        //   userId:
        public string GetUserName(string userId)
        {
            string commandText = "Select Name from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", userId } };
            return _database.GetStrValue(commandText, parameters);
        }

        //
        // Summary:
        //     Returns a User ID given a user name
        //
        // Parameters:
        //   userName:
        //     The user's name
        public string GetUserId(string userName)
        {
            string commandText = "Select Id from Users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", userName } };
            return _database.GetStrValue(commandText, parameters);
        }

        //
        // Summary:
        //     Returns an TUser given the user's id
        //
        // Parameters:
        //   userId:
        //     The user's id
        public TUser GetUserById(string userId)
        {
            TUser val = null;
            string commandText = "Select * from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", userId } };
            List<Dictionary<string, string>> list = _database.Query(commandText, parameters);
            if (list != null && list.Count == 1)
            {
                Dictionary<string, string> dictionary = list[0];
                val = (TUser)Activator.CreateInstance(typeof(TUser));
                val.Id = dictionary["Id"];
                val.UserName = dictionary["UserName"];
                val.PasswordHash = (string.IsNullOrEmpty(dictionary["PasswordHash"]) ? null : dictionary["PasswordHash"]);
                val.SecurityStamp = (string.IsNullOrEmpty(dictionary["SecurityStamp"]) ? null : dictionary["SecurityStamp"]);
                val.Email = (string.IsNullOrEmpty(dictionary["Email"]) ? null : dictionary["Email"]);
                val.EmailConfirmed = ((dictionary["EmailConfirmed"] == "1") ? true : false);
                val.PhoneNumber = (string.IsNullOrEmpty(dictionary["PhoneNumber"]) ? null : dictionary["PhoneNumber"]);
                val.PhoneNumberConfirmed = ((dictionary["PhoneNumberConfirmed"] == "1") ? true : false);
                val.LockoutEnabled = ((dictionary["LockoutEnabled"] == "1") ? true : false);
                val.LockoutEndDateUtc = (string.IsNullOrEmpty(dictionary["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(dictionary["LockoutEndDateUtc"]));
                val.AccessFailedCount = ((!string.IsNullOrEmpty(dictionary["AccessFailedCount"])) ? int.Parse(dictionary["AccessFailedCount"]) : 0);
            }

            return val;
        }

        //
        // Summary:
        //     Returns a list of TUser instances given a user name
        //
        // Parameters:
        //   userName:
        //     User's name
        public List<TUser> GetUserByName(string userName)
        {
            List<TUser> list = new List<TUser>();
            string commandText = "Select * from Users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", userName } };
            List<Dictionary<string, string>> list2 = _database.Query(commandText, parameters);
            foreach (Dictionary<string, string> item in list2)
            {
                TUser val = (TUser)Activator.CreateInstance(typeof(TUser));
                val.Id = item["Id"];
                val.UserName = item["UserName"];
                val.PasswordHash = (string.IsNullOrEmpty(item["PasswordHash"]) ? null : item["PasswordHash"]);
                val.SecurityStamp = (string.IsNullOrEmpty(item["SecurityStamp"]) ? null : item["SecurityStamp"]);
                val.Email = (string.IsNullOrEmpty(item["Email"]) ? null : item["Email"]);
                val.EmailConfirmed = ((item["EmailConfirmed"] == "1") ? true : false);
                val.PhoneNumber = (string.IsNullOrEmpty(item["PhoneNumber"]) ? null : item["PhoneNumber"]);
                val.PhoneNumberConfirmed = ((item["PhoneNumberConfirmed"] == "1") ? true : false);
                val.LockoutEnabled = ((item["LockoutEnabled"] == "1") ? true : false);
                val.TwoFactorEnabled = ((item["TwoFactorEnabled"] == "1") ? true : false);
                val.LockoutEndDateUtc = (string.IsNullOrEmpty(item["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(item["LockoutEndDateUtc"]));
                val.AccessFailedCount = ((!string.IsNullOrEmpty(item["AccessFailedCount"])) ? int.Parse(item["AccessFailedCount"]) : 0);
                list.Add(val);
            }

            return list;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            return null;
        }

        //
        // Summary:
        //     Return the user's password hash
        //
        // Parameters:
        //   userId:
        //     The user's id
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from Users where Id = @id";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@id", userId);
            string strValue = _database.GetStrValue(commandText, dictionary);
            if (string.IsNullOrEmpty(strValue))
            {
                return null;
            }

            return strValue;
        }

        //
        // Summary:
        //     Sets the user's password hash
        //
        // Parameters:
        //   userId:
        //
        //   passwordHash:
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update Users set PasswordHash = @pwdHash where Id = @id";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@pwdHash", passwordHash);
            dictionary.Add("@id", userId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Returns the user's security stamp
        //
        // Parameters:
        //   userId:
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", userId } };
            return _database.GetStrValue(commandText, parameters);
        }

        //
        // Summary:
        //     Inserts a new user in the Users table
        //
        // Parameters:
        //   user:
        public int Insert(TUser user)
        {
            string commandText = "Insert into Users (UserName, Id, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled)\r\n                values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled)";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@name", user.UserName);
            dictionary.Add("@id", user.Id);
            dictionary.Add("@pwdHash", user.PasswordHash);
            dictionary.Add("@SecStamp", user.SecurityStamp);
            dictionary.Add("@email", user.Email);
            dictionary.Add("@emailconfirmed", user.EmailConfirmed);
            dictionary.Add("@phonenumber", user.PhoneNumber);
            dictionary.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            dictionary.Add("@accesscount", user.AccessFailedCount);
            dictionary.Add("@lockoutenabled", user.LockoutEnabled);
            dictionary.Add("@lockoutenddate", user.LockoutEndDateUtc);
            dictionary.Add("@twofactorenabled", user.TwoFactorEnabled);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Deletes a user from the Users table
        //
        // Parameters:
        //   userId:
        //     The user's id
        private int Delete(string userId)
        {
            string commandText = "Delete from Users where Id = @userId";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@userId", userId);
            return _database.Execute(commandText, dictionary);
        }

        //
        // Summary:
        //     Deletes a user from the Users table
        //
        // Parameters:
        //   user:
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        //
        // Summary:
        //     Updates a user in the Users table
        //
        // Parameters:
        //   user:
        public int Update(TUser user)
        {
            string commandText = "Update Users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, \r\n                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,\r\n                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  \r\n                WHERE Id = @userId";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@userName", user.UserName);
            dictionary.Add("@pswHash", user.PasswordHash);
            dictionary.Add("@secStamp", user.SecurityStamp);
            dictionary.Add("@userId", user.Id);
            dictionary.Add("@email", user.Email);
            dictionary.Add("@emailconfirmed", user.EmailConfirmed);
            dictionary.Add("@phonenumber", user.PhoneNumber);
            dictionary.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            dictionary.Add("@accesscount", user.AccessFailedCount);
            dictionary.Add("@lockoutenabled", user.LockoutEnabled);
            dictionary.Add("@lockoutenddate", user.LockoutEndDateUtc);
            dictionary.Add("@twofactorenabled", user.TwoFactorEnabled);
            return _database.Execute(commandText, dictionary);
        }
    }
}
