namespace Salus.Services.UserProfileServices
{
    public class UserProfileService : IUserProfileService
    {
        public GenericService<Auth> _genericServicesAuth;
        public GenericService<UserProfile> _genericServicesUserProfile;
        Auth? auth;

        public UserProfileService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericServicesAuth = new(dataContext, httpContextAccessor);
            _genericServicesUserProfile = new(dataContext, httpContextAccessor);
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new Exception("You must log in first.");
        }
        //public methods
        public UserProfile SetProfilePicture(UserSetProfilePictureRequset request)
        {
            var userProfile = _genericServicesAuth.GetAuthenticatedUserProfile();

            userProfile.hairIndex = request.hairIndex == hairEnum.nondefined ? userProfile.hairIndex : request.hairIndex;
            userProfile.skinIndex = request.skinIndex == skinEnum.nondefined ? userProfile.skinIndex : request.skinIndex;
            userProfile.eyesIndex = request.eyesIndex == eyesEnum.nondefined ? userProfile.eyesIndex : request.eyesIndex;
            userProfile.mouthIndex = request.mouthIndex == mouthEnum.nondefined ? userProfile.mouthIndex : request.mouthIndex;
            
            var checkResult = CheckProfilePicture(userProfile);
            if (checkResult != "Everything's okay.")
                throw new Exception(checkResult);
            userProfile = _genericServicesUserProfile.Update(userProfile);
            return userProfile;
        }

        public UserProfile CreateProfile(UserSetDatasRequest request)
        {
            var checkResult = CheckCreateRequest(request);
            if (checkResult != "Everything's okay.")
                throw new Exception(checkResult);


            var userProfile = new UserProfile();
            userProfile.auth = auth;


            userProfile.weight = request.weight;
            userProfile.height = request.height;
            userProfile.birthDate = request.birthDate.ToString("yyyy.MM.dd.");
            userProfile.gender = request.gender;
            userProfile.goalWeight = request.goalWeight == 0 ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;
            userProfile = _genericServicesUserProfile.Create(userProfile);
            return userProfile;
        }

        public UserProfile ModifyProfile(UserSetDatasRequest request)
        {
            var checkResult = CheckUpdateRequest(request);
            if (checkResult != "Everything's okay.")
                throw new Exception(checkResult);

            var userProfile = _genericServicesUserProfile.GetAuthenticatedUserProfile();

            userProfile.weight = request.weight == default(double) ? userProfile.weight : request.weight;
            userProfile.height = request.height == default(double) ? userProfile.weight : request.weight;
            userProfile.birthDate = request.birthDate == default(DateTime) ? userProfile.birthDate : request.birthDate.ToString("yyyy.MM.dd");
            userProfile.gender = request.gender == default(genderEnum) ? userProfile.gender : request.gender;
            userProfile.goalWeight = request.goalWeight == default(double) ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;
            userProfile = _genericServicesUserProfile.Update(userProfile);
            return userProfile;
        }


        //private methods
        private string CheckCreateRequest(UserSetDatasRequest request)
        {
            if (request.birthDate < DateTime.Now.AddYears(-100) || request.birthDate > DateTime.Now.AddYears(-12))
                return $"The user must be between 12 and 100 years old!";

            if (request.weight < 20 || request.weight > 1000)
                return "The user weight must be over 20 and 1000!";

            if (request.height < 40 || request.height > 250)
                return "The user height must be between 40 and 250 cm!";

            if (request.gender == genderEnum.nondefined)
                return "You must select your gender!";

            if (request.gender < genderEnum.nondefined || request.gender > genderEnum.other)
                return "Invalid gender!";

            if (request.goalWeight != default(double) &&
                (request.goalWeight < 20 || request.goalWeight > 1000))
                return "The user goal weight must be over 20 and 1000!";

            return "Everything's okay.";
        }
        private string CheckUpdateRequest(UserSetDatasRequest request)
        {
            if (request.birthDate != default(DateTime) &&
                     (request.birthDate < DateTime.Now.AddYears(-100) || request.birthDate > DateTime.Now.AddYears(-12)))
                return $"The user must be between 12 and 100 years old!";

            if (request.weight != default(double) &&
                (request.weight < 20 || request.weight > 1000))
                return "The user weight must be over 20 and 1000!";

            if (request.height != default(double) &&
                (request.height < 40 || request.height > 250))
                return "The user height must be between 40 and 250 cm!";

            if (request.gender < genderEnum.nondefined || request.gender > genderEnum.other)
                return "Invalid gender!";

            if (request.goalWeight != default(double) &&
                (request.goalWeight < 20 || request.goalWeight > 1000))
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
