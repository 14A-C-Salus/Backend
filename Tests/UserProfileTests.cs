using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Salus.Controllers.Models.AuthModels;
using Salus.Controllers.Models.UserProfileModels;
using Salus.Data;
using Salus.Services.AuthServices;
using Salus.Services.UserProfileServices;
using System.Net.Http.Headers;

namespace Tests
{
    public class UserProfileTests:IDisposable
    {
        private readonly UserProfileService _userProfileService;
        protected readonly IConfiguration _configuration;

        private readonly AuthService _authService;



        private readonly DataContext _dataContext;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

        private readonly Auth _auth;
        private readonly AuthRegisterRequest _registerRequest;

        private readonly UserProfile _userProfile;
        private readonly UserSetDatasRequest _userSetDatasRequest;

        public UserProfileTests()
        {
            //try to make await
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

            _configuration = InitConfiguration();

            _dataContext = new DataContext(_configuration);

            //register and login
            _authService = new AuthService(_httpContextAccessorMock.Object, _dataContext, _configuration);

            var password = RandomString(12);
            _registerRequest = new AuthRegisterRequest()
            {
                username = $"{RandomString(12)}",
                email = $"{RandomString(12)}@address.com",
                password = password,
                confirmPassword = password
            };

            _auth = RegisterAndVerify().Result;

            var jwt = _authService.Login(new AuthLoginRequest()
            {
                email = _registerRequest.email,
                password = _registerRequest.password,
            }).Result;
            Login();

            //Create valid User Profile
            _userProfileService = new UserProfileService(_dataContext, _authService);
            _userSetDatasRequest = new UserSetDatasRequest()
            {
                birthDate = DateTime.Now.AddYears(-20),
                gender = genderEnum.male,
                height = 180,
                weight = 75
            };
            _userProfile = _userProfileService.CreateProfile(_userSetDatasRequest).Result;
        }

        private void Login()
        {
            
        }

        private async Task<Auth> RegisterAndVerify()
        {
            var auth = _authService.Register(_registerRequest).Result;
            _ = await _authService.Verify(auth.verificationToken);
            return auth;
        }

        public void Dispose()
        {
            _dataContext.auths.Remove(_auth);
            _dataContext.SaveChanges();
        }

        [Fact]
        public void CreateProfile()
        {
            Assert.Equal(_userProfile.height, _userSetDatasRequest.height);
            Assert.Equal(_userProfile.weight, _userSetDatasRequest.weight);
            Assert.Equal(_userProfile.birthDate, _userSetDatasRequest.birthDate.ToString("yyyy.MM.dd."));
            Assert.Equal(_userProfile.gender, _userSetDatasRequest.gender);
            Assert.NotEqual(default, _userProfile.goalWeight);
        }

        //        //not Fact methods
        //        private static UserSetDatasRequest CreateValidUserProfileRequest()
        //        {
        //            return new UserSetDatasRequest()
        //            {
        //                birthDate = DateTime.Now.AddYears(-20),
        //                gender = genderEnum.male,
        //                height = 175,
        //                weight = 80,
        //                goalWeight = 60
        //            };
        //        }

        //        private static UserSetDatasRequest CreateValidUserProfileRequestForUpdate()
        //        {
        //            return new UserSetDatasRequest()
        //            {
        //                birthDate = DateTime.Now.AddYears(-25),
        //                gender = genderEnum.female,
        //                height = 170
        //            };
        //        }

        //        private static UserSetProfilePictureRequset CreateValidProfilePictureRequest()
        //        {
        //            return new UserSetProfilePictureRequset()
        //            {
        //                eyesIndex = eyesEnum.blue,
        //                hairIndex = hairEnum.blond,
        //                mouthIndex = mouthEnum.happy,
        //                skinIndex = skinEnum.light
        //            };
        //        }
        //        private static UserSetProfilePictureRequset CreateValidProfilePictureRequestForUpdate()
        //        {
        //            return new UserSetProfilePictureRequset()
        //            {
        //                eyesIndex = eyesEnum.green,
        //                hairIndex = hairEnum.ginger
        //            };
        //        }
        
        
        
        private static string RandomString(int length)
        {
            var chars = Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 8);

            var randomStr = new string(chars.SelectMany(str => str)
                                            .OrderBy(c => Guid.NewGuid())
                                            .Take(length).ToArray());
            return randomStr;
        }

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }
        //        private static AuthRegisterRequest CreateValidAuthRegisterRequest()
        //        {
        //            return new AuthRegisterRequest()
        //            {
        //                email = "test@emailaddress.test",
        //                username = "testtest",
        //                password = "testtest",
        //                confirmPassword = "testtest"
        //            };
        //        }
    }
}