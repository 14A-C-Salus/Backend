using Microsoft.Extensions.Configuration;
using Salus.Controllers.Models.AuthModels;
using Salus.Controllers.Models.UserProfileModels;
using Salus.Data;
using Salus.Services.AuthServices;
using Salus.Services.UserProfileServices;

namespace Tests
{
    public class UserProfileTests
    {
        private readonly UserProfileService _userProfileService;
        protected readonly IConfiguration _configuration;

        private readonly AuthService _authService;

        private readonly DataContext _dataContext;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

        public UserProfileTests()
        {
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
            _configuration = InitConfiguration();
            _dataContext = new DataContext(_configuration);
            _authService = new AuthService(_httpContextAccessorMock.Object, _dataContext, _configuration);
            _userProfileService = new UserProfileService();
        }

        [Fact]
        private void CreateUserProfile()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var request = CreateValidUserProfileRequest();

            var userProfile = _userProfileService.SetUserProfileData(request, null, auth);

            Assert.False(userProfile.isAdmin);
            Assert.Equal(auth.id, userProfile.authOfProfileId);
            Assert.Equal(userProfile.height, request.height);
            Assert.Equal(userProfile.weight, request.weight);
            Assert.Equal(userProfile.goalWeight, request.goalWeight);
            Assert.Equal(userProfile.gender, request.gender);
            Assert.Equal(userProfile.birthDate, request.birthDate.ToString("yyyy.MM.dd."));
        }


        [Fact]
        private void UpdateUserProfile()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var oldRequest = CreateValidUserProfileRequest();
            var userProfile = _userProfileService.SetUserProfileData(oldRequest, null, auth);
            var newRequest = CreateValidUserProfileRequestForUpdate();

            //Update the birthDate, gender, height and auto generate the new goalWeight
            userProfile = _userProfileService.SetUserProfileData(newRequest, userProfile, auth);

            Assert.False(userProfile.isAdmin);
            Assert.Equal(auth.id, userProfile.authOfProfileId);
            Assert.Equal(userProfile.weight, oldRequest.weight);
            Assert.NotEqual(userProfile.goalWeight, oldRequest.goalWeight);

            Assert.Equal(userProfile.height, newRequest.height);
            Assert.Equal(userProfile.birthDate, newRequest.birthDate.ToString("yyyy.MM.dd."));
            Assert.Equal(userProfile.gender, newRequest.gender);
        }

        [Fact]
        private void CreateUserProfileWithEmptyRequest()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var request = new UserSetDatasRequest();
            Exception ex = Assert.Throws<Exception>(() => _userProfileService.SetUserProfileData(request, null, auth));
            Assert.Equal("Invalid userProfile", ex.Message);
        }

        [Fact]
        private void CreateProfilePicture()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var userProfileRequest = CreateValidUserProfileRequest();
            var profilePictureRequest = CreateValidProfilePictureRequest();
            var userProfile = _userProfileService.SetUserProfileData(userProfileRequest, null, auth);

            userProfile = _userProfileService.SetUserProfilePicture(profilePictureRequest, userProfile);

            Assert.Equal(userProfile.eyesIndex, profilePictureRequest.eyesIndex);
            Assert.Equal(userProfile.hairIndex, profilePictureRequest.hairIndex);
            Assert.Equal(userProfile.mouthIndex, profilePictureRequest.mouthIndex);
            Assert.Equal(userProfile.skinIndex, profilePictureRequest.skinIndex);
        }

        [Fact]
        private void UpdateProfilePicture()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var userProfileRequest = CreateValidUserProfileRequest();
            var oldProfilePictureRequest = CreateValidProfilePictureRequest();
            var newProfilePictureRequest = CreateValidProfilePictureRequestForUpdate();
            var userProfile = _userProfileService.SetUserProfileData(userProfileRequest, null, auth);

            userProfile = _userProfileService.SetUserProfilePicture(oldProfilePictureRequest, userProfile);
            //Update the eyes and the hair
            userProfile = _userProfileService.SetUserProfilePicture(newProfilePictureRequest, userProfile);

            Assert.Equal(userProfile.eyesIndex, newProfilePictureRequest.eyesIndex);
            Assert.Equal(userProfile.hairIndex, newProfilePictureRequest.hairIndex);
            Assert.NotEqual(userProfile.eyesIndex, oldProfilePictureRequest.eyesIndex);
            Assert.NotEqual(userProfile.hairIndex, oldProfilePictureRequest.hairIndex);
            Assert.Equal(userProfile.mouthIndex, oldProfilePictureRequest.mouthIndex);
            Assert.Equal(userProfile.skinIndex, oldProfilePictureRequest.skinIndex);
        }

        [Fact]
        private void CreateProfilePictureWithEmpty()
        {
            var auth = _authService.NewAuth(CreateValidAuthRegisterRequest());
            var userProfileRequest = CreateValidUserProfileRequest();
            var profilePictureRequest = new UserSetProfilePictureRequset();
            var userProfile = _userProfileService.SetUserProfileData(userProfileRequest, null, auth);

            Exception ex = Assert.Throws<Exception>(() => _userProfileService.SetUserProfilePicture(profilePictureRequest, userProfile));
            Assert.Equal("Invalid profile picture.", ex.Message);
        }

        //not Fact methods
        private static UserSetDatasRequest CreateValidUserProfileRequest()
        {
            return new UserSetDatasRequest()
            {
                birthDate = DateTime.Now.AddYears(-20),
                gender = genderEnum.male,
                height = 175,
                weight = 80,
                goalWeight = 60
            };
        }

        private static UserSetDatasRequest CreateValidUserProfileRequestForUpdate()
        {
            return new UserSetDatasRequest()
            {
                birthDate = DateTime.Now.AddYears(-25),
                gender = genderEnum.female,
                height = 170
            };
        }

        private static UserSetProfilePictureRequset CreateValidProfilePictureRequest()
        {
            return new UserSetProfilePictureRequset()
            {
                eyesIndex = eyesEnum.blue,
                hairIndex = hairEnum.blond,
                mouthIndex = mouthEnum.happy,
                skinIndex = skinEnum.light
            };
        }
        private static UserSetProfilePictureRequset CreateValidProfilePictureRequestForUpdate()
        {
            return new UserSetProfilePictureRequset()
            {
                eyesIndex = eyesEnum.green,
                hairIndex = hairEnum.ginger
            };
        }
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }
        private static AuthRegisterRequest CreateValidAuthRegisterRequest()
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