namespace backend.Models.DTOs.Recipe
{
    public class FavoriteRecipeDTO
    {
        public int Id { get; set; }
        public string MealId { get; set; }
        public string Title { get; set; }      // Retrieved from TheMealDB
        public string Category { get; set; }   // Retrieved from TheMealDB
        public string Thumbnail { get; set; }  // Retrieved from TheMealDB
        public DateTime AddedAt { get; set; }
    }
}
