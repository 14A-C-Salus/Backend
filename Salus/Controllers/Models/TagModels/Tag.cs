namespace Salus.Controllers.Models.TagModels
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;

        //Connection
        public List<UsersPreferTags> usersWhoPrefer { get; set; }
        public List<FoodsHaveTags> foodsThatHave { get; set; }
    }
}
