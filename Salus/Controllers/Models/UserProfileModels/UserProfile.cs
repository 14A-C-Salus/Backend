﻿using Newtonsoft.Json;

namespace Salus.Controllers.Models.UserProfileModels
{
    public class UserProfile
    {
        public int id { get; set; }
        public double weight { get; set; }
        public double height { get; set; }
        public string birthDate { get; set; } = default(DateTime).ToString("yyyy.MM.dd.");
        public genderEnum gender { get; set; }
        public bool isAdmin { get; set; } = false;
        public double goalWeight { get; set; }

        //Profile picture data
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }

        //Connections
        [Required, JsonIgnore]
        public Auth? auth { get; set; }
        [JsonIgnore]
        public int authOfProfileId { get; set; }
        [JsonIgnore]
        public IList<Following>? followerUserProfileToUserProfiles { get; set; }
        [JsonIgnore]
        public IList<Following>? followedUserProfileToUserProfiles { get; set; }
        [JsonIgnore]
        public IList<Comment>? commenterUserProfileToUserProfiles { get; set; }
        [JsonIgnore]
        public IList<Comment>? commentedUserProfileToUserProfiles { get; set; }

    }
}
