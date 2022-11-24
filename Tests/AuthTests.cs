using Microsoft.Extensions.Configuration;
using Moq;
using MySqlConnector;
using Salus.Controllers.Models.AuthModels;
using Salus.Data;
using Salus.Services.AuthServices;
using System.Data.Entity.Migrations;


namespace Tests
{
    public class AuthTests
    {
        string connectionString;
        string dbName;
        private readonly IConfiguration configuration;

        public AuthTests()
        {
            configuration = InitConfiguration();
        }

        [Fact]
        public void NewAuth()
        {
            var request = new AuthRegisterRequest()
            {
                email = "bela@bela.bela",
                username = "belabela",
                password = "belabela",
                confirmPassword = "belabela"
            };
            Auth auth = new Auth();
            var mockAuthServices = new Mock<IAuthService>();
            mockAuthServices.Setup(x => x.NewAuth(request)).Returns(auth);

            Assert.Equal("belabela", auth.username);
            Assert.Equal("bela@bela.bela", auth.email);
            Assert.Null(auth.date);
            Assert.Null(auth.passwordResetToken);
            Assert.Null(auth.resetTokenExpires);
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