namespace Salus.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        UserProfile CreateProfile(UserSetDatasRequest request);
        UserProfile GetProfile(int id);
        UserProfile ModifyProfile(UserSetDatasRequest request);
        UserProfile SetProfilePicture(UserSetProfilePictureRequset request);
    }
}
