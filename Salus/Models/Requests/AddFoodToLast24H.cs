﻿namespace Salus.Models.Requests
{
    public class AddFoodToLast24H
    {
        public bool isLiquid { get; set; }
        public int recipeId { get; set; }
        public int portion { get; set; }
        public int dl { get; set; }
    }
}
