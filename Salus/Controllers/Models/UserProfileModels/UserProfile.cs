using Newtonsoft.Json;

namespace Salus.Controllers.Models.UserProfileModels
{
    public class UserProfile
    {
        public int id { get; set; }
        public double weight { get; set; }
        public double height { get; set; }
        public string birthDate { get; set; } = default(DateTime).ToString("yyyy.MM.dd.");
        public genderEnum gender { get; set; }
        public double goalWeight { get; set; }

        //Profile picture data
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }

        //Connections
        [Required, JsonIgnore]
        public Auth? auth { get; set; }
        public Last24h? last24h { get; set; }
        [JsonIgnore]
        public int authOfProfileId { get; set; }
        [JsonIgnore]
        public IList<Following>? followers { get; set; }
        [JsonIgnore]
        public IList<Following>? followeds { get; set; }
        [JsonIgnore]
        public IList<Comment>? commenters { get; set; }
        [JsonIgnore]
        public IList<Comment>? commenteds { get; set; }
        public IList<UsersLikeRecipes>? likedRecipes { get; set; }
        public IList<UsersPreferTags>? preferredTags { get; set; }
    }
}
