using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Salus.Controllers.Models.AuthModels;
using Salus.Controllers.Models.UserProfileModels;
using Salus.Data;
using Salus.Services.AuthServices;
using Salus.Services.UserProfileServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        void CreateUserProfile()
        {
            var userProfile = _userProfileService.SetUserProfileData(CreateValidUserProfileRequest(), null, 1);
            Assert.False(userProfile.isAdmin);
            Assert.Equal(1, userProfile.authOfProfileId);
            Assert.Equal(userProfile.height, CreateValidUserProfileRequest().height);
            Assert.Equal(userProfile.weight, CreateValidUserProfileRequest().weight);
            Assert.Equal(userProfile.goalWeight, CreateValidUserProfileRequest().goalWeight);
            Assert.Equal(userProfile.gender, CreateValidUserProfileRequest().gender);
        }

        [Fact]
        void CheckData()
        {
            var userProfile = _userProfileService.SetUserProfileData(CreateValidUserProfileRequest(), null, 1);
            Assert.Equal("Everything's okay.", _userProfileService.CheckData(userProfile));
        }
        [Fact]
        void CreateProfilePicture()
        {
            var userProfile = _userProfileService.SetUserProfileData(CreateValidUserProfileRequest(), null, 1);
            userProfile = _userProfileService.SetUserProfilePicture(CreateValidProfilePictureRequest(), userProfile);
            Assert.Equal(userProfile.eyesIndex, CreateValidProfilePictureRequest().eyesIndex);
            Assert.Equal(userProfile.hairIndex, CreateValidProfilePictureRequest().hairIndex);
            Assert.Equal(userProfile.mouthIndex, CreateValidProfilePictureRequest().mouthIndex);
            Assert.Equal(userProfile.skinIndex, CreateValidProfilePictureRequest().skinIndex);
        }
        [Fact]
        void CheckProfilePicture()
        {
            var userProfile = _userProfileService.SetUserProfileData(CreateValidUserProfileRequest(), null, 1);
            userProfile = _userProfileService.SetUserProfilePicture(CreateValidProfilePictureRequest(), userProfile);
            Assert.Equal("Everything's okay.", _userProfileService.CheckProfilePicture(userProfile));
        }
        //private methods
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
    }
}