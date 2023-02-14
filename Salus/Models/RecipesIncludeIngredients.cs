﻿using Newtonsoft.Json;

namespace Salus.Controllers.Models.JoiningEntity
{
    public class RecipesIncludeIngredients
    {
        public int id { get; set; }
        public int portionInGramm { get; set; }
        public int recipeId { get; set; }
        [Required, JsonIgnore]
        public Recipe? recipe { get; set; }
        public int foodId { get; set; }
        [Required, JsonIgnore]
        public Food? food { get; set; }
    }
}