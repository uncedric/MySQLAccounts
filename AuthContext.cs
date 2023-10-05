
namespace MySQLAccounts
{
    public class AuthContext : MySQLDatabase
    {
        public AuthContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
    }
}
