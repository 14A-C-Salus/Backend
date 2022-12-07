using System.ComponentModel.DataAnnotations;

namespace Salus.Controllers.Models.SocialMediaModels
{
    public class ModifyCommentRequest
    {
        [Required]
        public int commentId { get; set; }
        
        [MaxLength(1000), Required]
        public string body { get; set; } = string.Empty;
    }
}
