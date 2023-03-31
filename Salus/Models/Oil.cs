﻿namespace Salus.Controllers.Models.RecipeModels
{
    public class Oil
    {
        public int id { get; set; }
        public int calIn14Ml { get; set; }
        public string name { get; set; } = string.Empty;
        public virtual IList<Recipe>? recipes { get; set; }
    }
}
