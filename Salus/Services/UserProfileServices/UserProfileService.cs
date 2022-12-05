namespace Salus.Services.UserProfileServices
{
    public class UserProfileService : IUserProfileService
    {
        //public methods
        public UserProfile SetUserProfileData(UserSetDatasRequest request, UserProfile? userProfile, Auth auth)
        {
            if (userProfile == null)
            {
                userProfile = new UserProfile();
                userProfile.isAdmin = false;
                userProfile.auth = auth;
            }

            userProfile.weight = request.weight == 0 ? userProfile.weight : request.weight;
            userProfile.height = request.height == 0 ? userProfile.height : request.height;
            userProfile.birthDate = request.birthDate == default(DateTime) ? userProfile.birthDate : request.birthDate.ToString("yyyy.MM.dd.");
            userProfile.gender = request.gender == genderEnum.nondefined ? userProfile.gender : request.gender;
            userProfile.goalWeight = request.goalWeight == 0 ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;
            if (CheckData(userProfile) != "Everything's okay.")
                throw new Exception("Invalid userProfile");
            return userProfile;
        }

        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile)
        {
            userProfile.hairIndex = request.hairIndex == hairEnum.nondefined ? userProfile.hairIndex : request.hairIndex;
            userProfile.skinIndex = request.skinIndex == skinEnum.nondefined ? userProfile.skinIndex : request.skinIndex;
            userProfile.eyesIndex = request.eyesIndex == eyesEnum.nondefined ? userProfile.eyesIndex : request.eyesIndex;
            userProfile.mouthIndex = request.mouthIndex == mouthEnum.nondefined ? userProfile.mouthIndex : request.mouthIndex;
            if (CheckProfilePicture(userProfile) != "Everything's okay.")
                throw new Exception("Invalid profile picture.");
            return userProfile;
        }


        //private methods
        private string CheckData(UserProfile userProfile)
        {
            if (Convert.ToDateTime(userProfile.birthDate) < DateTime.Now.AddYears(-100) || Convert.ToDateTime(userProfile.birthDate) > DateTime.Now.AddYears(-12))
                return $"The user must be between 12 and 100 years old!";

            if (userProfile.weight < 20 || userProfile.weight > 1000)
                return "The user weight must be over 20 and 1000!";

            if (userProfile.height < 40 || userProfile.height > 250)
                return "The user height must be between 40 and 250 cm!";

            if (userProfile.gender == genderEnum.nondefined)
                return "You must select your gender!";

            if (userProfile.gender < genderEnum.nondefined || userProfile.gender > genderEnum.other)
                return "Invalid gender!";

            if (userProfile.goalWeight < 20 || userProfile.goalWeight > 1000)
                return "The user goal weight must be over 20 and 1000!";

            return "Everything's okay.";
        }
        private string CheckProfilePicture(UserProfile userProfile)
        {
            if (userProfile.hairIndex < hairEnum.nondefined || userProfile.hairIndex > hairEnum.white)
                return "Invalid hair!";

            if (userProfile.skinIndex < skinEnum.nondefined || userProfile.skinIndex > skinEnum.lightest)
                return "Invalid skin color!";

            if (userProfile.eyesIndex < eyesEnum.nondefined || userProfile.eyesIndex > eyesEnum.brown)
                return "Invalid eye color!";

            if (userProfile.mouthIndex < mouthEnum.nondefined || userProfile.mouthIndex > mouthEnum.sad)
                return "Invalid mouth!";

            if (userProfile.hairIndex == hairEnum.nondefined)
                return "Select a hair!";

            if (userProfile.skinIndex == skinEnum.nondefined)
                return "Select a skin color!";

            if (userProfile.eyesIndex == eyesEnum.nondefined)
                return "Select a eye!";

            if (userProfile.mouthIndex == mouthEnum.nondefined)
                return "Select a mouth!";

            return "Everything's okay.";
        }

        private double SetGoalWeight(double height, double weight)
        {
            double heightInMeter = height / 100;

            double minimumIdealBMI = 18.5;
            double maximumIdealBMI = 25;

            double idealBMI = (minimumIdealBMI + maximumIdealBMI) / 2;
            double BMI = weight / (heightInMeter * heightInMeter);

            if (BMI > minimumIdealBMI && BMI < maximumIdealBMI)
                return weight;

            return idealBMI * heightInMeter * heightInMeter;
        }
    }
}
