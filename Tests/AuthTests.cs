using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Salus.Controllers.Models.AuthModels;
using Salus.Data;
using Salus.Services.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var request = new AuthRegisterRequest();
            Assert.ThrowsAny<Exception>(() => _authService.NewAuth(request));
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

        [Fact]
        public void CreateTokenWithValidData()
        {
            var auth = new Auth()
            {
                email = "notvalidemail@exist.not",
                username = "Just need to test"
            };
            var jwt = _authService.CreateToken(auth);
            Assert.Equal("notvalidemail@exist.not", GetEmailfromJwt(jwt));
        }

        [Fact]
        public void CreateTokenWithEmptyAuth()
        {
            var auth = new Auth();
            Assert.Throws<Exception>(() => _authService.CreateToken(auth));
        }

        [Fact]
        public void VerifyPasswordHash()
        {
            //Todo
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

        public string GetEmailfromJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            List<Claim> claims = securityToken.Claims.ToList();
            return claims[1].Value.ToString();
        }

        private AuthRegisterRequest CreateValidAuthRegisterRequest()
        {
            return new AuthRegisterRequest()
            {
                //todo
            };
        }
    }
}