using System.ComponentModel.DataAnnotations;

namespace Authentication.Controllers.Models.SocialMediaModels
{
    public class UnFollowFollowRequest
    {
        [EmailAddress, Required]
        public string email { get; set; } = string.Empty;
    }
}
