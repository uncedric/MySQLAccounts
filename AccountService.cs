using JL.MySQLAccounts;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLAccounts

public class AccountService
{
    private AuthContext _ctx;

    private UserManager<IdentityUser> _userManager;

    //
    // Parameters:
    //   connectionStringName:
    public AccountService(string connectionStringName)
    {
        _ctx = new AuthContext(connectionStringName);
        _userManager = new z<IdentityUser>(new UserStore<IdentityUser>(_ctx));
    }

    public void Dispose()
    {
        _ctx.Dispose();
        _userManager.Dispose();
    }

    //
    // Parameters:
    //   user:
    //
    //   password:
    public async Task<IdentityResult> RegisterUser(IdentityUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    //
    // Parameters:
    //   id:
    //
    //   newPassword:
    public async Task ResetPassword(string id, string newPassword)
    {
        try
        {
            UserStore<IdentityUser> store = new UserStore<IdentityUser>(_ctx);
            string hashedNewPassword = _userManager.PasswordHasher.HashPassword(newPassword);
            IdentityUser cUser = await _userManager.FindByIdAsync(id);
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   id:
    //
    //   password:
    //
    //   newPassword:
    public async Task<IdentityResult> ChangePassword(string id, string password, string newPassword)
    {
        try
        {
            return await _userManager.ChangePasswordAsync(id, password, newPassword);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   user:
    public async Task<IdentityResult> CreateAsync(IdentityUser user)
    {
        try
        {
            return await _userManager.CreateAsync(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    //
    //   login:
    public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
    {
        try
        {
            return await _userManager.AddLoginAsync(userId, login);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userName:
    //
    //   password:
    public async Task<IdentityUser> FindUser(string userName, string password)
    {
        try
        {
            return await _userManager.FindAsync(userName, password);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task<IdentityUser> FindUser(string userId)
    {
        try
        {
            return await _userManager.FindByIdAsync(userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   loginInfo:
    public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
    {
        try
        {
            return await _userManager.FindAsync(loginInfo);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   email:
    public async Task<IdentityUser> FindUserByEmail(string email)
    {
        try
        {
            return await _userManager.FindByNameAsync(email);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task<bool> IsLockedout(string userId)
    {
        try
        {
            return await _userManager.IsLockedOutAsync(userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task AccessFailed(string userId)
    {
        try
        {
            await _userManager.AccessFailedAsync(userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task ResetAccessFailedCount(string userId)
    {
        try
        {
            await _userManager.ResetAccessFailedCountAsync(userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task<int> GetAccessFailedCount(string userId)
    {
        try
        {
            return await _userManager.GetAccessFailedCountAsync(userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task<bool> GetLockoutEnabled(string userId)
    {
        try
        {
            return await _userManager.GetLockoutEnabledAsync(userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    public async Task RemoveUserRoles(string userId)
    {
        try
        {
            List<string> list = _userManager.GetRoles(userId).ToList();
            foreach (string item in list)
            {
                await _userManager.RemoveFromRoleAsync(userId, item);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    //
    // Parameters:
    //   userId:
    //
    //   role:
    public async Task AddUserToRole(string userId, string role)
    {
        try
        {
            await _userManager.AddToRoleAsync(userId, role);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

}
