using Microsoft.Extensions.Configuration;
using Salus.Controllers.Models.AuthModels;
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
        private void NewAuthWithRequest()
        {
            var request = CreateValidAuthRegisterRequest();

            var auth = _authService.NewAuth(request);

            Assert.Equal(request.username, auth.username);
            Assert.Equal(request.email, auth.email);
            Assert.Null(auth.date);
            Assert.Null(auth.passwordResetToken);
            Assert.Null(auth.resetTokenExpires);
        }

        [Fact]
        private void NewAuthWithEmptyRequest()
        {
            var request = new AuthRegisterRequest();
            Assert.ThrowsAny<Exception>(() => _authService.NewAuth(request));
        }

        [Fact]
        private void SendEmailToInvalidAddress()
        {
            var auth = new Auth()
            {
                email = "notvadfgdfgdfglidemail@exist.not",
                username = "Just need to test",
                verificationToken = "Also need to test"
            };

            Exception ex = Assert.Throws<Exception>(() => _authService.SendToken(auth));

            Assert.Equal("Email address doesn't exist.", ex.Message);
        }

        [Fact]
        private void CreateTokenWithValidData()
        {
            var request = CreateValidAuthRegisterRequest();
            var auth = _authService.NewAuth(request);

            var jwt = _authService.CreateToken(auth);

            Assert.Equal(request.email, GetEmailfromJwt(jwt));
        }

        [Fact]
        private void CreateTokenWithEmptyAuth()
        {
            var auth = new Auth();
            Assert.Throws<Exception>(() => _authService.CreateToken(auth));
        }

        [Fact]
        private void VerifyPasswordHashWithValidData()
        {
            var request = CreateValidAuthRegisterRequest();
            var auth = _authService.NewAuth(request);

            Assert.True(_authService.VerifyPasswordHash(request.password, auth.passwordHash, auth.passwordSalt));
        }

        [Fact]
        private void VerifyPasswordHashWithWrongPassword()
        {
            var request = CreateValidAuthRegisterRequest();
            var auth = _authService.NewAuth(request);
            var wrongPassword = "wrongpassword";

            Assert.False(_authService.VerifyPasswordHash(wrongPassword, auth.passwordHash, auth.passwordSalt));
        }

        [Fact]
        private void SetTokenAndExpires()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());

            _authService.SetTokenAndExpires(auth);

            Assert.NotNull(auth.resetTokenExpires);
            Assert.NotNull(auth.passwordResetToken);
        }


        [Fact]
        private void SetTokenAndExpiresnWithEmptyAuth()
        {
            var auth = new Auth();

            Assert.Throws<Exception>(() => _authService.SetTokenAndExpires(auth));

            Assert.Null(auth.resetTokenExpires);
            Assert.Null(auth.passwordResetToken);
        }

        [Fact]
        private void UpdateAuthResetPasswordData()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var oldPasswordHash = auth.passwordHash;
            var oldPasswordSalt = auth.passwordSalt;

            _authService.SetTokenAndExpires(auth);
            _authService.UpdateAuthResetPasswordData("newpassword", auth);

            Assert.Null(auth.resetTokenExpires);
            Assert.Null(auth.passwordResetToken);
            Assert.NotEqual(oldPasswordHash, auth.passwordHash);
            Assert.NotEqual(oldPasswordSalt, auth.passwordSalt);
        }

        [Fact]
        private void UpdateAuthResetPasswordDataWithoutSetTokenAndExpires()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var oldPasswordHash = auth.passwordHash;
            var oldPasswordSalt = auth.passwordSalt;

            Exception ex = Assert.Throws<Exception>(() => _authService.UpdateAuthResetPasswordData("newpassword", auth));

            Assert.Equal("You need first use the 'forgoted-password' service!", ex.Message);
            Assert.Null(auth.resetTokenExpires);
            Assert.Null(auth.passwordResetToken);
            Assert.Equal(oldPasswordHash, auth.passwordHash);
            Assert.Equal(oldPasswordSalt, auth.passwordSalt);
        }

        //not Fact methods
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }

        private string GetEmailfromJwt(string token)
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
                email = "test@emailaddress.test",
                username = "testtest",
                password = "testtest",
                confirmPassword = "testtest"
            };
        }
    }
}