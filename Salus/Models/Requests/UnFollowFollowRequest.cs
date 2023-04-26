using System.ComponentModel.DataAnnotations;

namespace Salus.Models.Requests
{
    public class UnFollowFollowRequest
    {
        [Required]
        public int id { get; set; }
    }
}
