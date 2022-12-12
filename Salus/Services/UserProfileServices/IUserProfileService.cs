namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        Task<string> CreateProfile(UserSetDatasRequest request);
        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile);
    }
}
