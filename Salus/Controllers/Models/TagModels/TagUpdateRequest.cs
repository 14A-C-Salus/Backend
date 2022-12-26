namespace Salus.Controllers.Models.TagModels
{
    public class TagUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
}
