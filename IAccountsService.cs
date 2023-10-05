using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MySQLAccounts
{
    public interface IAccountsService
    {
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);

        Task<IdentityResult> ChangePassword(string id, string password, string newPassword);

        Task<IdentityResult> CreateAsync(IdentityUser user);

        Task<IdentityResult> RegisterUser(IdentityUser user, string password);

        Task<IdentityUser> FindAsync(UserLoginInfo loginInfo);

        Task<IdentityUser> FindUser(string userId);

        Task<IdentityUser> FindUser(string userName, string password);

        Task<IdentityUser> FindUserByEmail(string email);

        Task<bool> IsLockedout(string userId);

        Task AccessFailed(string userId);

        Task ResetAccessFailedCount(string userId);

        Task<int> GetAccessFailedCount(string userId);

        Task<bool> GetLockoutEnabled(string userId);

        Task ResetPassword(string id, string newPassword);

        Task AddUserToRole(string userId, string role);

        Task RemoveUserRoles(string userId);
    }
}