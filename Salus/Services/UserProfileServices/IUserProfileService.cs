namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateProfile(UserSetDatasRequest request);
        Task<UserProfile> ModifyProfile(UserSetDatasRequest request);
        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile);
    }
}
