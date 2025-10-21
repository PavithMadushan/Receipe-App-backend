namespace backend.Models.DTOs.Recipe
{
    // ✅ Basic Recipe data used when listing recipes
    public class RecipeDTO
    {
        public string MealId { get; set; }       // TheMealDB ID
        public string Title { get; set; }        // strMeal
        public string Category { get; set; }     // strCategory
        public string Area { get; set; }         // strArea (e.g., "Italian")
        public string Thumbnail { get; set; }    // strMealThumb
    }
}
