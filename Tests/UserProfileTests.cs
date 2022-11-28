using Salus.Controllers.Models.UserProfileModels;
using Salus.Services.UserProfileServices;

namespace Tests
{
    public class UserProfileTests
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileTests()
        {
            var context = new DefaultHttpContext();

            _userProfileService = new UserProfileService();
        }

        [Fact]
        private void CreateUserProfile()
        {
            var request = CreateValidUserProfileRequest();

            var userProfile = _userProfileService.SetUserProfileData(request, null, 1);

            Assert.False(userProfile.isAdmin);
            Assert.Equal(1, userProfile.authOfProfileId);
            Assert.Equal(userProfile.height, request.height);
            Assert.Equal(userProfile.weight, request.weight);
            Assert.Equal(userProfile.goalWeight, request.goalWeight);
            Assert.Equal(userProfile.gender, request.gender);
            Assert.Equal(userProfile.birthDate, request.birthDate.ToString("yyyy.MM.dd."));
        }


        [Fact]
        private void UpdateUserProfile()
        {
            var oldRequest = CreateValidUserProfileRequest();
            var newRequest = CreateValidUserProfileRequestForUpdate();
            var userProfile = _userProfileService.SetUserProfileData(oldRequest, null, 1);

            //Update the birthDate, gender, height and auto generate the new goalWeight
            userProfile = _userProfileService.SetUserProfileData(newRequest, userProfile, 1);

            Assert.False(userProfile.isAdmin);
            Assert.Equal(1, userProfile.authOfProfileId);
            Assert.Equal(userProfile.weight, oldRequest.weight);
            Assert.NotEqual(userProfile.goalWeight, oldRequest.goalWeight);

            Assert.Equal(userProfile.height, newRequest.height);
            Assert.Equal(userProfile.birthDate, newRequest.birthDate.ToString("yyyy.MM.dd."));
            Assert.Equal(userProfile.gender, newRequest.gender);
        }

        [Fact]
        private void CreateUserProfileWithEmptyRequest()
        {
            var request = new UserSetDatasRequest();
            Exception ex = Assert.Throws<Exception>(() => _userProfileService.SetUserProfileData(request, null, 1));
            Assert.Equal("Invalid userProfile", ex.Message);
        }

        [Fact]
        private void CreateProfilePicture()
        {
            var userProfileRequest = CreateValidUserProfileRequest();
            var profilePictureRequest = CreateValidProfilePictureRequest();
            var userProfile = _userProfileService.SetUserProfileData(userProfileRequest, null, 1);

            userProfile = _userProfileService.SetUserProfilePicture(profilePictureRequest, userProfile);

            Assert.Equal(userProfile.eyesIndex, profilePictureRequest.eyesIndex);
            Assert.Equal(userProfile.hairIndex, profilePictureRequest.hairIndex);
            Assert.Equal(userProfile.mouthIndex, profilePictureRequest.mouthIndex);
            Assert.Equal(userProfile.skinIndex, profilePictureRequest.skinIndex);
        }

        [Fact]
        private void UpdateProfilePicture()
        {
            var userProfileRequest = CreateValidUserProfileRequest();
            var oldProfilePictureRequest = CreateValidProfilePictureRequest();
            var newProfilePictureRequest = CreateValidProfilePictureRequestForUpdate();
            var userProfile = _userProfileService.SetUserProfileData(userProfileRequest, null, 1);

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
            var userProfileRequest = CreateValidUserProfileRequest();
            var profilePictureRequest = new UserSetProfilePictureRequset();
            var userProfile = _userProfileService.SetUserProfileData(userProfileRequest, null, 1);

            Exception ex = Assert.Throws<Exception>(() => _userProfileService.SetUserProfilePicture(profilePictureRequest, userProfile));
            Assert.Equal("Invalid profile picture.", ex.Message);
        }

        //not Fact methods
        private UserSetDatasRequest CreateValidUserProfileRequest()
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

        private UserSetDatasRequest CreateValidUserProfileRequestForUpdate()
        {
            return new UserSetDatasRequest()
            {
                birthDate = DateTime.Now.AddYears(-25),
                gender = genderEnum.female,
                height = 170
            };
        }

        private UserSetProfilePictureRequset CreateValidProfilePictureRequest()
        {
            return new UserSetProfilePictureRequset()
            {
                eyesIndex = eyesEnum.blue,
                hairIndex = hairEnum.blond,
                mouthIndex = mouthEnum.happy,
                skinIndex = skinEnum.light
            };
        }
        private UserSetProfilePictureRequset CreateValidProfilePictureRequestForUpdate()
        {
            return new UserSetProfilePictureRequset()
            {
                eyesIndex = eyesEnum.green,
                hairIndex = hairEnum.ginger
            };
        }
    }
}