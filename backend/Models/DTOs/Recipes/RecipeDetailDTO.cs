namespace backend.Models.DTOs.Recipe
{
    // ✅ Full Recipe detail when viewing a single recipe
    public class RecipeDetailDTO
    {
        public string MealId { get; set; }                // TheMealDB ID
        public string Title { get; set; }                 // strMeal
        public string Category { get; set; }              // strCategory
        public string Area { get; set; }                  // strArea (e.g., "Italian")
        public string Instructions { get; set; }          // strInstructions
        public string Thumbnail { get; set; }             // strMealThumb
        public string YoutubeLink { get; set; }           // strYoutube (if available)
        public List<string>? Ingredients { get; set; }    // List of ingredients
        public List<string>? Measures { get; set; }       // Corresponding measures
    }
}
