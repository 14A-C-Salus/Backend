using Salus.Models;
using Salus.Models.Requests;

namespace Salus.Services.Interfaces
{
    public interface IUserProfileService
    {
        UserProfile RemoveDiet();
        List<Diet> GetRecommendedDiets();
        UserProfile CreateProfile(UserSetDatasRequest request);
        UserProfile GetProfile(int id);
        UserProfile ModifyProfile(UserSetDatasRequest request);
        UserProfile SetProfilePicture(UserSetProfilePictureRequset request);
        UserProfile AddDiet(int dietId);
        List<UserProfile> GetUserprofilesByName(string name);
    }
}
