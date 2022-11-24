using Microsoft.Extensions.Configuration;
using Moq;
using MySqlConnector;
using Salus.Controllers.Models.AuthModels;
using Salus.Data;
using Salus.Services.AuthServices;
using System.Data.Entity.Migrations;


namespace Tests
{
    public class SqlTests : IDisposable
    {
        string connectionString;
        string dbName;
        private readonly IConfiguration configuration;

        public SqlTests()
        {
            Init();
            configuration = InitConfiguration();
        }

        private void Init()
        {
            dbName = $"salus_{Guid.NewGuid()}";
            while (dbName.Contains("-"))
                dbName = dbName.Replace("-", "_");

            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = $"CREATE DATABASE {dbName} CHARACTER SET utf8 COLLATE utf8_hungarian_ci";
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            var connStrProps = DataContext.GetConnectionString(configuration).Split(";").ToList();
            foreach (var prop in connStrProps)
            {
                if (prop.Contains("Database="))
                {
                    var updatedDatabaseProperty = $"Database={dbName}";
                    connStrProps.Insert(connStrProps.IndexOf(prop), updatedDatabaseProperty);
                    connStrProps.Remove(prop);
                    break;
                }
            }

            connectionString = string.Join(";", connStrProps);

            using (var conn = new MySqlConnection(connectionString))
            {
                Migrate();

                var sql = "insert into dbo.auths (id, username, email, passwordHash, passwordSalt, verificationToken, date) \n" +
                      "value (1, 'Sándor', 'sandor@sandor.sandor', 'nincs', 'nincs', 'nincs', 'nincs')";

                conn.Open();

                using (var cmd = new MySqlCommand(sql, conn))
                    cmd.ExecuteNonQuery();
            }
        }

        private void Migrate()
        {
            var conf = new DbMigrationsConfiguration();
            var migrator = new DbMigrator(conf);
            migrator.Update();
        }

        public void Dispose()
        {
            connectionString = DataContext.GetConnectionString(configuration);

            using (var conn = new MySqlConnection(connectionString))
            {
                var sql = $"DROP DATABASE {dbName}";
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
    }
}