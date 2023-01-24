using Newtonsoft.Json;

namespace Salus.Controllers.Models.JoiningEntity
{
    public class FoodsHaveTags
    {
        [Required, JsonIgnore]
        public Food food { get; set; }
        public int foodId { get; set; }
        [Required, JsonIgnore]
        public Tag tag { get; set; }
        public int tagId { get; set; }
    }
}
