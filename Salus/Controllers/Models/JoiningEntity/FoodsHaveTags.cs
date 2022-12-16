namespace Salus.Controllers.Models.JoiningEntity
{
    public class FoodsHaveTags
    {
        public int foodId { get; set; }
        public Food food { get; set; }
        public int tagId { get; set; }
        public Tag tag { get; set; }
    }
}
