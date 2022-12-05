namespace Salus.Controllers.Models.UserProfileModels
{
    public class UserProfile
    {
        public int id { get; set; }
        public double weight { get; set; }
        public double height { get; set; }
        public string birthDate { get; set; } = default(DateTime).ToString("yyyy.MM.dd");
        public genderEnum gender { get; set; }
        public bool isAdmin { get; set; } = false;
        public double goalWeight { get; set; }

        //Profile picture data
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }

        //Connections
        public virtual Auth? auth { get; set; }
        public int authOfProfileId { get; set; }
        public IList<Following>? followerUserProfileToUserProfiles { get; set; }
        public IList<Following>? followedUserProfileToUserProfiles { get; set; }
    }
}
