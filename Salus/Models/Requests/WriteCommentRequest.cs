using System.ComponentModel.DataAnnotations;

namespace Salus.Models.Requests
{
    public class WriteCommentRequest
    {
        [EmailAddress, Required]
        public string email { get; set; } = string.Empty;
        
        [MaxLength(1000), Required]
        public string body { get; set; } = string.Empty;
    }
}
