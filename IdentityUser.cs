using System;
using Microsoft.AspNet.Identity;

namespace MySQLAccounts
{
    //
    // Summary:
    //     Class that implements the ASP.NET Identity IUser interface
    public class IdentityUser : IUser, IUser<string>
    {
        //
        // Summary:
        //     User ID
        public string Id { get; set; }

        //
        // Summary:
        //     User's name
        public string UserName { get; set; }

        //
        // Summary:
        //     Email
        public virtual string Email { get; set; }

        //
        // Summary:
        //     True if the email is confirmed, default is false
        public virtual bool EmailConfirmed { get; set; }

        //
        // Summary:
        //     The salted/hashed form of the user password
        public virtual string PasswordHash { get; set; }

        //
        // Summary:
        //     A random value that should change whenever a users credentials have changed (password
        //     changed, login removed)
        public virtual string SecurityStamp { get; set; }

        //
        // Summary:
        //     PhoneNumber for the user
        public virtual string PhoneNumber { get; set; }

        //
        // Summary:
        //     True if the phone number is confirmed, default is false
        public virtual bool PhoneNumberConfirmed { get; set; }

        //
        // Summary:
        //     Is two factor enabled for the user
        public virtual bool TwoFactorEnabled { get; set; }

        //
        // Summary:
        //     DateTime in UTC when lockout ends, any time in the past is considered not locked
        //     out.
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        //
        // Summary:
        //     Is lockout enabled for this user
        public virtual bool LockoutEnabled { get; set; }

        //
        // Summary:
        //     Used to record failures for the purposes of lockout
        public virtual int AccessFailedCount { get; set; }

        //
        // Summary:
        //     Default constructor
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        //
        // Summary:
        //     Constructor that takes user name as argument
        //
        // Parameters:
        //   userName:
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }
    }
}

