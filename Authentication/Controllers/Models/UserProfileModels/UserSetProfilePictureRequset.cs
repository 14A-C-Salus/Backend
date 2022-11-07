using System.ComponentModel.DataAnnotations;

namespace Authentication.Controllers.Models.UserProfileModels
{
    public class UserSetProfilePictureRequset
    {
        [Required(ErrorMessage = "You must log in first!"), EmailAddress]
        public string email { get; set; } = string.Empty;
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }
    }
}
