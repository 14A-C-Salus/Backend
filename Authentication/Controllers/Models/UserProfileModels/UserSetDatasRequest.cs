using System.ComponentModel.DataAnnotations;

namespace Authentication.Controllers.Models.UserProfileModels
{
    public class UserSetDatasRequest
    {
        [Required(ErrorMessage = "You must log in first!"), EmailAddress]
        public string email { get; set; } = string.Empty;
        public double weight { get; set; }
        public double height { get; set; }
        public DateTime birthDate { get; set; } = default(DateTime);
        public genderEnum gender { get; set; }
        public double goalWeight { get; set; }
    }
}
