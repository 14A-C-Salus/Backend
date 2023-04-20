using System.ComponentModel.DataAnnotations;

namespace Salus.Models.Requests
{
    public class UnFollowFollowRequest
    {
        [EmailAddress, Required]
        public string email { get; set; } = string.Empty;
    }
}
