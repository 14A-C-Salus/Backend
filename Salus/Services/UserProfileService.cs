using Salus.Exceptions;
using Salus.Models;

namespace Salus.Services.UserProfileServices
{
    public class UserProfileService : IUserProfileService
    {
        public GenericService<Auth> _genericServicesAuth;
        public GenericService<UserProfile> _genericServicesUserProfile;
        public GenericService<Diet> _genericServicesDiet;
        Auth? auth;

        public UserProfileService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericServicesAuth = new(dataContext, httpContextAccessor);
            _genericServicesUserProfile = new(dataContext, httpContextAccessor);
            _genericServicesDiet = new(dataContext, httpContextAccessor);
        }

        public List<Diet> GetRecommendedDiets()
        {
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new ELoginRequired();
            var userProfile = _genericServicesAuth.GetAuthenticatedUserProfile();
            List<Diet> diets = new();
            foreach (var diet in _genericServicesDiet.ReadAll())
            {
                if (diet.maxKcal != null && diet.maxKcal < userProfile.maxKcal)
                {
                    diets.Add(diet);
                }
            }
            return diets;
        }

        public UserProfile AddDiet(int dietId)
        {
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new ELoginRequired();
            var userProfile = _genericServicesAuth.GetAuthenticatedUserProfile();

            var diet = _genericServicesDiet.Read(dietId);
            if (diet == null)
                throw new EInvalidDiet();

            userProfile.diet = diet;
            return _genericServicesUserProfile.Update(userProfile);
        }

        public UserProfile RemoveDiet()
        {
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new ELoginRequired();
            var userProfile = _genericServicesAuth.GetAuthenticatedUserProfile();

            userProfile.diet = null;
            return _genericServicesUserProfile.Update(userProfile);
        }
        //public methods
        public UserProfile SetProfilePicture(UserSetProfilePictureRequset request)
        {
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new ELoginRequired();
            var userProfile = _genericServicesAuth.GetAuthenticatedUserProfile();

            userProfile.hairIndex = request.hairIndex == hairEnum.nondefined ? userProfile.hairIndex : request.hairIndex;
            userProfile.skinIndex = request.skinIndex == skinEnum.nondefined ? userProfile.skinIndex : request.skinIndex;
            userProfile.eyesIndex = request.eyesIndex == eyesEnum.nondefined ? userProfile.eyesIndex : request.eyesIndex;
            userProfile.mouthIndex = request.mouthIndex == mouthEnum.nondefined ? userProfile.mouthIndex : request.mouthIndex;
            
            CheckProfilePicture(userProfile);

            userProfile = _genericServicesUserProfile.Update(userProfile);
            return userProfile;
        }

        public UserProfile GetProfile(int id)
        {
            var userProfile = _genericServicesUserProfile.Read(id);
            if (userProfile == null)
                throw new EInvalidUserProfilId();
            return userProfile;
        }

        public UserProfile CreateProfile(UserSetDatasRequest request)
        {
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new ELoginRequired();
            CheckCreateRequest(request);
            var userProfile = new UserProfile();
            userProfile.auth = auth;
            userProfile.weight = request.weight;
            userProfile.height = request.height;
            userProfile.birthDate = request.birthDate;
            userProfile.gender = request.gender;
            userProfile.goalWeight = request.goalWeight == 0 ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;
            userProfile = _genericServicesUserProfile.Create(userProfile);
            return userProfile;
        }

        public UserProfile ModifyProfile(UserSetDatasRequest request)
        {
            auth = _genericServicesAuth.Read(_genericServicesAuth.GetAuthId());
            if (auth == null)
                throw new ELoginRequired();
            CheckUpdateRequest(request);
            var userProfile = _genericServicesUserProfile.GetAuthenticatedUserProfile();
            userProfile.weight = request.weight == default(double) ? userProfile.weight : request.weight;
            userProfile.height = request.height == default(double) ? userProfile.weight : request.weight;
            userProfile.birthDate = request.birthDate == default(DateTime) ? userProfile.birthDate : request.birthDate;
            userProfile.gender = request.gender == default(genderEnum) ? userProfile.gender : request.gender;
            userProfile.goalWeight = request.goalWeight == default(double) ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;
            userProfile = _genericServicesUserProfile.Update(userProfile);
            return userProfile;
        }


        //private methods
        private void CheckCreateRequest(UserSetDatasRequest request)
        {
            if (request.birthDate < DateTime.Now.AddYears(-100) || request.birthDate > DateTime.Now.AddYears(-12))
                throw new EInvalidBirthDate();

            if (request.weight < 20 || request.weight > 1000)
                throw new EInvalidWeight();

            if (request.height < 40 || request.height > 250)
                throw new EInvalidHeight();

            if (request.gender == genderEnum.nondefined)
                throw new EGenderNotSelected();

            if (request.gender < genderEnum.nondefined || request.gender > genderEnum.female)
                throw new EInvalidGender();

            if (request.goalWeight != default(double) &&
                (request.goalWeight < 20 || request.goalWeight > 1000))
                throw new EInvalidGoalWeight();
        }
        private void CheckUpdateRequest(UserSetDatasRequest request)
        {
            if (request.birthDate != default(DateTime) &&
                     (request.birthDate < DateTime.Now.AddYears(-100) || request.birthDate > DateTime.Now.AddYears(-12)))
                throw new EInvalidBirthDate();

            if (request.weight != default(double) &&
                (request.weight < 20 || request.weight > 1000))
                throw new EInvalidWeight();
            if (request.height != default(double) &&
                (request.height < 40 || request.height > 250))
                throw new EInvalidHeight();

            if (request.gender < genderEnum.nondefined || request.gender > genderEnum.female)
                throw new EInvalidGender();

            if (request.goalWeight != default(double) &&
                (request.goalWeight < 20 || request.goalWeight > 1000))
                throw new EInvalidGoalWeight();
        }
        private void CheckProfilePicture(UserProfile userProfile)
        {
            if (userProfile.hairIndex < hairEnum.nondefined || userProfile.hairIndex > hairEnum.white)
                throw new EInvalidHair();

            if (userProfile.skinIndex < skinEnum.nondefined || userProfile.skinIndex > skinEnum.lightest)
                throw new EInvalidSkin();

            if (userProfile.eyesIndex < eyesEnum.nondefined || userProfile.eyesIndex > eyesEnum.brown)
                throw new EInvalidEye();

            if (userProfile.mouthIndex < mouthEnum.nondefined || userProfile.mouthIndex > mouthEnum.sad)
                throw new EInvalidMouth();

            if (userProfile.hairIndex == hairEnum.nondefined)
                throw new EInvalidHair();

            if (userProfile.skinIndex == skinEnum.nondefined)
                throw new EInvalidSkin();

            if (userProfile.eyesIndex == eyesEnum.nondefined)
                throw new EInvalidEye();

            if (userProfile.mouthIndex == mouthEnum.nondefined)
                throw new EInvalidMouth();
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
