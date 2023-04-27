using Newtonsoft.Json;

namespace Salus.Models
{
    public class RecepiesHaveTags
    {
        [Required, JsonIgnore]
        public Recipe? recipe { get; set; }
        public int recipeId { get; set; }
        [Required]
        public Tag? tag { get; set; }
        public int tagId { get; set; }
    }
}
