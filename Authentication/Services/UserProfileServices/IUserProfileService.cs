namespace Authentication.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        public string CheckData(UserProfile userProfile);
        public string CheckProfilePicture(UserProfile userProfile);
        public UserProfile SetUserProfileData(UserSetDatasRequest request, UserProfile? userProfile, int id);
        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile);
    }
}
