using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Salus.Controllers.Models.AuthModels;
using Salus.Services.AuthServices;
using Salus.Data;

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
        public void NewAuth()
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

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}