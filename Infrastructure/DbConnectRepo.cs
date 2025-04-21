using Interfaces;

namespace Infrastructure
{
    public class DbConnectRepo:IDbConnectRepo
    {
        private readonly string _connectionString;

        public DbConnectRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string GetDatabaseConnection()
        {
            return _connectionString;
        }
    }
}
    