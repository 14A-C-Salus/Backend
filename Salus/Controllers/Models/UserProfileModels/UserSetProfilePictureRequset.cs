using System.ComponentModel.DataAnnotations;

namespace Salus.Controllers.Models.UserProfileModels
{
    public class UserSetProfilePictureRequset
    {
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }
    }
}
