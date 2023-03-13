namespace Salus.Models
{
    public class Diet
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public Between<int>? fat { get; set; }
        public Between<int>? carbohydrate { get; set; }
        public Between<int>? protein { get; set; }
        public Between<int>? kcal { get; set; }
        public int minDl { get; set; }
        public virtual IList<UserProfile>? userProfiles { get; set; }
    }
}
