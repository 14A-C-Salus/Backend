using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly DataContext _dataContext;
        public UserProfileController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpPost("set-data")]
        public async Task<IActionResult> SetData(UserSetDatasRequest request)
        {
            var auth = await _dataContext.auths.FirstOrDefaultAsync(a => a.email == request.email);
            if (auth == null)
                return BadRequest("You most log in first.");
            var userProfile = await _dataContext.userProfiles.FirstOrDefaultAsync(u => u.authOfProfileId == auth.id);        
            bool isNew = userProfile == null;
            if (isNew)
            {
                userProfile = new UserProfile();
                userProfile.isAdmin = false;
                userProfile.authOfProfileId = auth.id;
            }
            userProfile.weight = request.weight == 0 ? userProfile.weight : request.weight;
            userProfile.height = request.height == 0 ? userProfile.height : request.height;
            userProfile.birthDate = request.birthDate == default(DateTime) ? userProfile.birthDate : request.birthDate.ToString("yyyy.MM.dd.");
            userProfile.gender = request.gender == genderEnum.nondefined ? userProfile.gender : request.gender;
            userProfile.goalWeight = request.goalWeight == 0 ? SetGoalWeight(userProfile.height, userProfile.weight) : request.goalWeight;
            var check = CheckData(userProfile);
            if (check != "Everything's okay.")
                return BadRequest(check);
            if (isNew)
                _dataContext.userProfiles.Add(userProfile);
            await _dataContext.SaveChangesAsync();
            return Ok($"{auth.username}'s data saved. Gender: {userProfile.gender}. Goal Weight: {userProfile.goalWeight}.");
        }

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
            if (userProfile.goalWeight < 20 || userProfile.goalWeight > 1000)
                return "The user goal weight must be over 20 and 1000!";
            return "Everything's okay.";
        }

        private double SetGoalWeight(double height, double weight)
        {
            double heightInMeter = height/100;
            double BMI = weight/(heightInMeter * heightInMeter);
            if (BMI > 18.5 && BMI < 25)
            {
                return weight;
            }
            double idealBMI = 21.75;
            return idealBMI * heightInMeter * heightInMeter;
        }
    }
}
