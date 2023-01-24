namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        UserProfile CreateProfile(UserSetDatasRequest request);
        UserProfile ModifyProfile(UserSetDatasRequest request);
        UserProfile SetProfilePicture(UserSetProfilePictureRequset request);
    }
}
