using System.ComponentModel.DataAnnotations;

namespace Salus.Controllers.Models.SocialMediaModels
{
    public class UnFollowFollowRequest
    {
        [EmailAddress, Required]
        public string email { get; set; } = string.Empty;
    }
}
