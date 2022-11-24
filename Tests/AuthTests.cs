using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Salus.Controllers.Models.AuthModels;
using Salus.Services.AuthServices;
using Salus.Data;
using System.Net;

namespace Tests
{
    public class AuthTests 
    {
        protected readonly IConfiguration _configuration;

        private readonly AuthService _authService;

        private readonly DataContext _dataContext;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        public AuthTests()
        {
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
            _configuration = InitConfiguration();
            _dataContext = new DataContext(_configuration);
            _authService = new AuthService(_httpContextAccessorMock.Object, _dataContext, _configuration);
        }

        [Fact]
        public void NewAuthWithRequest()
        {
            var request = new AuthRegisterRequest()
            {
                email = "bela@bela.bela",
                username = "belabela",
                password = "belabela",
                confirmPassword = "belabela"
            };

            var auth = _authService.NewAuth(request);

            Assert.Equal("belabela", auth.username);
            Assert.Equal("bela@bela.bela", auth.email);
            Assert.Null(auth.date);
            Assert.Null(auth.passwordResetToken);
            Assert.Null(auth.resetTokenExpires);
        }

        [Fact]
        public void NewAuthWithEmptyRequest()
        {
            Assert.ThrowsAny<Exception>(()=> TryCreateNewAuthWithEmptyRequest());
        }


        [Fact]
        public void SendEmailToInvalidAddress()
        {
            var auth = new Auth()
            {
                email = "notvalidemail@exist.not",
                username = "Just need to test",
                verificationToken = "Also need to test"
            };
            Exception ex = Assert.Throws<Exception>(() => _authService.SendToken(auth));
            Assert.Equal("Email address doesn't exist.", ex.Message);
        }

        private void TryCreateNewAuthWithEmptyRequest()
        {
            var request = new AuthRegisterRequest();
            var auth = _authService.NewAuth(request);
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}