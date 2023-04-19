using System.ComponentModel.DataAnnotations;

namespace Salus.Models.Requests
{
    public class ModifyCommentRequest
    {
        [Required]
        public int commentId { get; set; }
        
        [MaxLength(1000), Required]
        public string body { get; set; } = string.Empty;
    }
}
