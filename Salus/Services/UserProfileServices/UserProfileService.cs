﻿namespace Salus.Services.UserProfileServices
{
    public class UserProfileService : IUserProfileService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;

        public UserProfileService(DataContext dataContext, IAuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }
        //public methods
        public async Task<string> CreateProfile(UserSetDatasRequest request)
        {
            var email = _authService.GetEmail();

            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                throw new Exception("You must log in first.");

            var checkResult = CheckData(request);
            if (checkResult != "Everything's okay.")
                throw new Exception(checkResult);


            var userProfile = new UserProfile();
            userProfile.isAdmin = false;
            userProfile.auth = auth;


            userProfile.weight = request.weight;
            userProfile.height = request.height;
            userProfile.birthDate = request.birthDate.ToString("yyyy.MM.dd.");
            userProfile.gender = request.gender;
            userProfile.goalWeight = request.goalWeight == 0 ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;

            await _dataContext.SaveChangesAsync();
            return $"{auth.username}'s data saved. Gender: {userProfile.gender}. Goal Weight: {userProfile.goalWeight}.";
        }

        public UserProfile SetUserProfilePicture(UserSetProfilePictureRequset request, UserProfile userProfile)
        {
            userProfile.hairIndex = request.hairIndex == hairEnum.nondefined ? userProfile.hairIndex : request.hairIndex;
            userProfile.skinIndex = request.skinIndex == skinEnum.nondefined ? userProfile.skinIndex : request.skinIndex;
            userProfile.eyesIndex = request.eyesIndex == eyesEnum.nondefined ? userProfile.eyesIndex : request.eyesIndex;
            userProfile.mouthIndex = request.mouthIndex == mouthEnum.nondefined ? userProfile.mouthIndex : request.mouthIndex;
            var checkResult = CheckProfilePicture(userProfile);
            if (checkResult != "Everything's okay.")
                throw new Exception(checkResult);
            return userProfile;
        }


        //private methods
        private string CheckData(UserSetDatasRequest request)
        {
            if (Convert.ToDateTime(request.birthDate) < DateTime.Now.AddYears(-100) || Convert.ToDateTime(request.birthDate) > DateTime.Now.AddYears(-12))
                return $"The user must be between 12 and 100 years old!";

            if (request.weight < 20 || request.weight > 1000)
                return "The user weight must be over 20 and 1000!";

            if (request.height < 40 || request.height > 250)
                return "The user height must be between 40 and 250 cm!";

            if (request.gender == genderEnum.nondefined)
                return "You must select your gender!";

            if (request.gender < genderEnum.nondefined || request.gender > genderEnum.other)
                return "Invalid gender!";

            if (request.goalWeight < 20 || request.goalWeight > 1000)
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
