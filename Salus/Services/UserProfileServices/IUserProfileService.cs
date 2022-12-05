namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        public UserProfile SetUserProfileData(UserSetDatasRequest request, UserProfile? userProfile, Auth auth);
        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile);
    }
}
