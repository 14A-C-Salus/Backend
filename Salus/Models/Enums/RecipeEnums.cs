namespace Salus.Models.Enums
{
    public class RecipeEnums
    {
        public enum makeingMethodEnum
        {
            nondefined,
            baking, //sütés, mint a kenyeret
            frying, //sütés, mint a sült krumplit
            roasting, //pirítás
            cooking //főzés
        }
        public enum recipePropertiesEnum
        {
            nondefined,
            kcal,
            protein,
            fat,
            carbohydrate
        }
    }
}
