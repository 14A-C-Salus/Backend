namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateProfile(UserSetDatasRequest request);
        Task<UserProfile> ModifyProfile(UserSetDatasRequest request);
        Task<UserProfile> SetProfilePicture(UserSetProfilePictureRequset request);
    }
}
