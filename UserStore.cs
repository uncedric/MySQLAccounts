
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JL.MySQLAccounts;
using Microsoft.AspNet.Identity;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that implements the key ASP.NET Identity user store iterfaces
    public class UserStore<TUser> : IUserLoginStore<TUser>, IUserLoginStore<TUser, string>, IUserStore<TUser, string>, IDisposable, IUserClaimStore<TUser>, IUserClaimStore<TUser, string>, IUserRoleStore<TUser>, IUserRoleStore<TUser, string>, IUserPasswordStore<TUser>, IUserPasswordStore<TUser, string>, IUserSecurityStampStore<TUser>, IUserSecurityStampStore<TUser, string>, IQueryableUserStore<TUser>, IQueryableUserStore<TUser, string>, IUserEmailStore<TUser>, IUserEmailStore<TUser, string>, IUserPhoneNumberStore<TUser>, IUserPhoneNumberStore<TUser, string>, IUserTwoFactorStore<TUser, string>, IUserLockoutStore<TUser, string>, IUserStore<TUser> where TUser : IdentityUser
    {
        private UserTable<TUser> userTable;

        private RoleTable roleTable;

        private UserRolesTable userRolesTable;

        private UserClaimsTable userClaimsTable;

        private UserLoginsTable userLoginsTable;

        public MySQLDatabase Database { get; private set; }

        public IQueryable<TUser> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //
        // Summary:
        //     Default constructor that initializes a new MySQLDatabase instance using the Default
        //     Connection string
        public UserStore()
        {
            new UserStore<TUser>(new MySQLDatabase());
        }

        //
        // Summary:
        //     Constructor that takes a MySQLDatabase as argument
        //
        // Parameters:
        //   database:
        public UserStore(MySQLDatabase database)
        {
            Database = database;
            userTable = new UserTable<TUser>(database);
            roleTable = new RoleTable(database);
            userRolesTable = new UserRolesTable(database);
            userClaimsTable = new UserClaimsTable(database);
            userLoginsTable = new UserLoginsTable(database);
        }

        //
        // Summary:
        //     Insert a new TUser in the UserTable
        //
        // Parameters:
        //   user:
        public Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            userTable.Insert(user);
            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Returns an TUser instance based on a userId query
        //
        // Parameters:
        //   userId:
        //     The user's Id
        public Task<TUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            TUser userById = userTable.GetUserById(userId);
            if (userById != null)
            {
                return Task.FromResult(userById);
            }

            return Task.FromResult<TUser>(null);
        }

        //
        // Summary:
        //     Returns an TUser instance based on a userName query
        //
        // Parameters:
        //   userName:
        //     The user's name
        public Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            List<TUser> userByName = userTable.GetUserByName(userName);
            if (userByName != null && userByName.Count == 1)
            {
                return Task.FromResult(userByName[0]);
            }

            return Task.FromResult<TUser>(null);
        }

        //
        // Summary:
        //     Updates the UsersTable with the TUser instance values
        //
        // Parameters:
        //   user:
        //     TUser to be updated
        public Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            userTable.Update(user);
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
                Database = null;
            }
        }

        //
        // Summary:
        //     Inserts a claim to the UserClaimsTable for the given user
        //
        // Parameters:
        //   user:
        //     User to have claim added
        //
        //   claim:
        //     Claim to be added
        public Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            userClaimsTable.Insert(claim, user.Id);
            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Returns all claims for a given user
        //
        // Parameters:
        //   user:
        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ClaimsIdentity claimsIdentity = userClaimsTable.FindByUserId(user.Id);
            return Task.FromResult((IList<Claim>)claimsIdentity.Claims.ToList());
        }

        //
        // Summary:
        //     Removes a claim froma user
        //
        // Parameters:
        //   user:
        //     User to have claim removed
        //
        //   claim:
        //     Claim to be removed
        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            userClaimsTable.Delete(user, claim);
            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Inserts a Login in the UserLoginsTable for a given User
        //
        // Parameters:
        //   user:
        //     User to have login added
        //
        //   login:
        //     Login to be added
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            userLoginsTable.Insert(user, login);
            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Returns an TUser based on the Login info
        //
        // Parameters:
        //   login:
        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            string text = userLoginsTable.FindUserIdByLogin(login);
            if (text != null)
            {
                TUser userById = userTable.GetUserById(text);
                if (userById != null)
                {
                    return Task.FromResult(userById);
                }
            }

            return Task.FromResult<TUser>(null);
        }

        //
        // Summary:
        //     Returns list of UserLoginInfo for a given TUser
        //
        // Parameters:
        //   user:
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            List<UserLoginInfo> list = new List<UserLoginInfo>();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<UserLoginInfo> list2 = userLoginsTable.FindByUserId(user.Id);
            if (list2 != null)
            {
                return Task.FromResult((IList<UserLoginInfo>)list2);
            }

            return Task.FromResult<IList<UserLoginInfo>>(null);
        }

        //
        // Summary:
        //     Deletes a login from UserLoginsTable for a given TUser
        //
        // Parameters:
        //   user:
        //     User to have login removed
        //
        //   login:
        //     Login to be removed
        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            userLoginsTable.Delete(user, login);
            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Inserts a entry in the UserRoles table
        //
        // Parameters:
        //   user:
        //     User to have role added
        //
        //   roleName:
        //     Name of the role to be added to user
        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            string roleId = roleTable.GetRoleId(roleName);
            if (!string.IsNullOrEmpty(roleId))
            {
                userRolesTable.Insert(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Returns the roles for a given TUser
        //
        // Parameters:
        //   user:
        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<string> list = userRolesTable.FindByUserId(user.Id);
            if (list != null)
            {
                return Task.FromResult((IList<string>)list);
            }

            return Task.FromResult<IList<string>>(null);
        }

        //
        // Summary:
        //     Verifies if a user is in a role
        //
        // Parameters:
        //   user:
        //
        //   role:
        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            List<string> list = userRolesTable.FindByUserId(user.Id);
            if (list != null && list.Contains(role))
            {
                return Task.FromResult(result: true);
            }

            return Task.FromResult(result: false);
        }

        //
        // Summary:
        //     Removes a user from a role
        //
        // Parameters:
        //   user:
        //
        //   role:
        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Deletes a user
        //
        // Parameters:
        //   user:
        public Task DeleteAsync(TUser user)
        {
            if (user != null)
            {
                userTable.Delete(user);
            }

            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Returns the PasswordHash for a given TUser
        //
        // Parameters:
        //   user:
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            string passwordHash = userTable.GetPasswordHash(user.Id);
            return Task.FromResult(passwordHash);
        }

        //
        // Summary:
        //     Verifies if user has password
        //
        // Parameters:
        //   user:
        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(bool.Parse((!string.IsNullOrEmpty(userTable.GetPasswordHash(user.Id))).ToString()));
        }

        //
        // Summary:
        //     Sets the password hash for a given TUser
        //
        // Parameters:
        //   user:
        //
        //   passwordHash:
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Set security stamp
        //
        // Parameters:
        //   user:
        //
        //   stamp:
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Get security stamp
        //
        // Parameters:
        //   user:
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        //
        // Summary:
        //     Set email on user
        //
        // Parameters:
        //   user:
        //
        //   email:
        public Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Get email from user
        //
        // Parameters:
        //   user:
        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        //
        // Summary:
        //     Get if user email is confirmed
        //
        // Parameters:
        //   user:
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        //
        // Summary:
        //     Set when user email is confirmed
        //
        // Parameters:
        //   user:
        //
        //   confirmed:
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Get user by email
        //
        // Parameters:
        //   email:
        public Task<TUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            TUser val = userTable.GetUserByEmail(email) as TUser;
            if (val != null)
            {
                return Task.FromResult(val);
            }

            return Task.FromResult<TUser>(null);
        }

        //
        // Summary:
        //     Set user phone number
        //
        // Parameters:
        //   user:
        //
        //   phoneNumber:
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Get user phone number
        //
        // Parameters:
        //   user:
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        //
        // Summary:
        //     Get if user phone number is confirmed
        //
        // Parameters:
        //   user:
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        //
        // Summary:
        //     Set phone number if confirmed
        //
        // Parameters:
        //   user:
        //
        //   confirmed:
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Set two factor authentication is enabled on the user
        //
        // Parameters:
        //   user:
        //
        //   enabled:
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Get if two factor authentication is enabled on the user
        //
        // Parameters:
        //   user:
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        //
        // Summary:
        //     Get user lock out end date
        //
        // Parameters:
        //   user:
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEndDateUtc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) : default(DateTimeOffset));
        }

        //
        // Summary:
        //     Set user lockout end date
        //
        // Parameters:
        //   user:
        //
        //   lockoutEnd:
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Increment failed access count
        //
        // Parameters:
        //   user:
        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            userTable.Update(user);
            return Task.FromResult(user.AccessFailedCount);
        }

        //
        // Summary:
        //     Reset failed access count
        //
        // Parameters:
        //   user:
        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            userTable.Update(user);
            return Task.FromResult(0);
        }

        //
        // Summary:
        //     Get failed access count
        //
        // Parameters:
        //   user:
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        //
        // Summary:
        //     Get if lockout is enabled for the user
        //
        // Parameters:
        //   user:
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        //
        // Summary:
        //     Set lockout enabled for user
        //
        // Parameters:
        //   user:
        //
        //   enabled:
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            userTable.Update(user);
            return Task.FromResult(0);
        }
    }
}