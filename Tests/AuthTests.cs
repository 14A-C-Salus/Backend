using Microsoft.Extensions.Configuration;
using Salus.Controllers.Models.AuthModels;
using Salus.Services.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Tests
{
    public class AuthTests : IDisposable
    {
        protected readonly IConfiguration _configuration;

        private readonly AuthService _authService;

        private readonly DataContext _dataContext;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Auth _auth;
        private readonly AuthRegisterRequest _request;
        public AuthTests()
        {
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
            _configuration = InitConfiguration();
            _dataContext = new DataContext(_configuration);
            _authService = new AuthService(_httpContextAccessorMock.Object, _dataContext, _configuration);
            _request = new AuthRegisterRequest()
            {
                username = "Test Username",
                email = "validemail@format.com",
                password = "validpassword",
                confirmPassword = "validpassword"
            };
            _auth = _authService.Register(_request).Result;
        }

        public void Dispose()
        {
            _dataContext.auths.Remove(_auth);
            _dataContext.SaveChanges();
        }

        [Fact]
        private void Register()
        {
            //before each
            Assert.Equal(_request.username, _auth.username);
            Assert.Equal(_request.email, _auth.email);
            Assert.NotNull(_auth.passwordHash);
            Assert.NotNull(_auth.passwordSalt);
            Assert.Null(_auth.date);
            Assert.Null(_auth.passwordResetToken);
            Assert.Null(_auth.resetTokenExpires);
        }

        [Fact]
        private async void Verify()
        {
            _ = await _authService.Verify(_auth.verificationToken);
            Assert.NotNull(_auth.date);
        }

        [Fact]
        private async void Login()
        {
            _ = await _authService.Verify(_auth.verificationToken);
            var loginRequest = new AuthLoginRequest()
            {
                email = _request.email,
                password = _request.password
            };
            var jwt = _authService.Login(loginRequest).Result;
            Assert.NotNull(jwt);
            Assert.Equal(loginRequest.email, GetEmailfromJwt(jwt));
        }

        [Fact]
        private async void ForgotPassword()
        {
            _ = await _authService.ForgotPassword(_auth.email);

            Assert.NotNull(_auth.passwordResetToken);
            Assert.NotNull(_auth.resetTokenExpires);
        }

        [Fact]
        private async void ResetPassword()
        {
            _ = await _authService.ForgotPassword(_auth.email);
            byte[][] oldPasswordHashAndSalt = {_auth.passwordHash, _auth.passwordSalt};
            
            var resetPasswordRequest = new AuthResetPasswordRequest()
            {
                token = _auth.passwordResetToken,
                password = "newpassword",
                confirmPassword = "newpassword"
            };
            _ = _authService.ResetPassword(resetPasswordRequest).Result;
            Assert.Null(_auth.passwordResetToken);
            Assert.Null(_auth.resetTokenExpires);
            Assert.NotEqual(_auth.passwordHash, oldPasswordHashAndSalt[0]);
            Assert.NotEqual(_auth.passwordSalt, oldPasswordHashAndSalt[1]);
        }

        //[Fact]
        //private void InvalidDataTestExample()
        //{
        //    Exception ex = Assert.Throws<Exception>(() => _authService.Method());
        //    Assert.Equal("Email address doesn't exist.", ex.Message);

        //    Assert.ThrowsAny<Exception>(() => _authService.NewAuth(request));
        //}

        //not Fact methods
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }

        private static string GetEmailfromJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

            List<Claim> claims = securityToken.Claims.ToList();

            return claims[1].Value.ToString();
        }
    }
}