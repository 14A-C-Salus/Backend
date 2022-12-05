using System.ComponentModel.DataAnnotations;

namespace Salus.Controllers.Models.JoiningEntity
{
    public class Comment
    {
        public int id { get; set; }
        public int fromId { get; set; }
        public int toId { get; set; }

        [Required]
        public virtual UserProfile? commentFrom { get; set; }
        [Required]
        public virtual UserProfile? commentTo { get; set; }
        [MaxLength(1000)]
        public string body { get; set; } = string.Empty;
        public string sendDate { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

    }
}
