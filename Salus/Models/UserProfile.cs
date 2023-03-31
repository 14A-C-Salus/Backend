using Newtonsoft.Json;
using Salus.Exceptions;
using Salus.Models;

namespace Salus.Controllers.Models.UserProfileModels
{
    public class UserProfile
    {
        public int id { get; set; }
        public double weight { get; set; }
        public double height { get; set; }
        public DateTime birthDate { get; set; }
        public genderEnum gender { get; set; }
        public double goalWeight { get; set; }
        public int? maxKcal {
            get
            {
                if (diet == null)
                {
                    if (gender == genderEnum.male)
                        return (int)((10 * goalWeight) + (6.25 * height) - (5 * (DateTime.Now.Year - birthDate.Year)) + 5);
                    else if (gender == genderEnum.female)
                        return (int)((10 * goalWeight) + (6.25 * height) - (5 * (DateTime.Now.Year - birthDate.Year)) - 161);
                    else
                        throw new EGenderNotSelected();
                }
                else
                {
                    return diet.maxKcal;
                }
            }
            set
            {

            }
        }
        public int? minDl 
        {
            get
            {
                if (diet == null)
                {
                    if (gender == genderEnum.male)
                        return 37;
                    if
                        (gender == genderEnum.female)
                        return 27;
                    throw new EGenderNotSelected();
                }
                else { return diet.minDl; }
            }
            set { }
        }

        //Profile picture data
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }

        //Connections
        [Required, JsonIgnore]
        public Auth? auth { get; set; }
        [JsonIgnore]
        public Last24h? last24h { get; set; }
        [JsonIgnore]
        public Diet? diet { get; set; }
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
        [JsonIgnore]
        public IList<UsersLikeRecipes>? likedRecipes { get; set; }
        [JsonIgnore]
        public IList<UsersPreferTags>? preferredTags { get; set; }
    }
}
