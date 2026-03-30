using Microsoft.EntityFrameworkCore;
using Timekeeper.Models;

namespace Timekeeper.Services
{
    public class TimekeeperContext : DbContext
    {
        private string _dbServer;
        private string _dbName;
        private string _dbUsername;
        private string _dbPassword;
        private string _connStr;

        public DbSet<Employee> Employees { get; set; }

        public DbSet<TimekeepingTransaction> TimeKeepingTransactions { get; set; }

        public DbSet<TransactionType> TransactionType { get; set; }

        public TimekeeperContext(DbContextOptions<TimekeeperContext> options) : base(options)
        {

        }

        [Obsolete]
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            return;
            DotNetEnv.Env.Load();

            _dbServer = DotNetEnv.Env.GetString("DB_SERVER");
            _dbName = DotNetEnv.Env.GetString("DB_NAME");
            _dbUsername = DotNetEnv.Env.GetString("DB_USERNAME");
            _dbPassword = DotNetEnv.Env.GetString("DB_PASSWORD");
            _connStr = $"Server={_dbServer};Database={_dbName};User Id={_dbUsername};Password={_dbPassword};TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(_connStr);
        }
    }
}
