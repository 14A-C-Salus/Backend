﻿namespace Salus.Controllers.Models.FoodModels
{
    public class Oil
    {
        public int id { get; set; }
        public int kcalIn14Ml { get; set; }
        public virtual List<Recipe> recipes { get; set; }
    }
}