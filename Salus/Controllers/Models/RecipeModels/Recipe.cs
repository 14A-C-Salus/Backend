namespace Salus.Controllers.Models.RecipeModels
{
    public class Recipe
    {
        public int id { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public string name { get; set; } = string.Empty;
        public bool verifeid { get; set; } = false;
        [Required]
        public UserProfile Author { get; set; } = new ();
        public List<Tag> tags { get; set; } = new ();

        //Connections
        [Required]
        public List<Food> ingredients { get; set; } = new ();
    }
}
