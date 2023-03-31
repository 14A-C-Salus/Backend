using Newtonsoft.Json;

namespace Salus.Controllers.Models.JoiningEntity
{
    public class RecepiesHaveTags
    {
        [Required, JsonIgnore]
        public Recipe? recipe { get; set; }
        public int recipeId { get; set; }
        [Required, JsonIgnore]
        public Tag? tag { get; set; }
        public int tagId { get; set; }
    }
}
