namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        public UserProfile SetUserProfileData(UserSetDatasRequest request, UserProfile? userProfile, int id);
        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile);
    }
}
