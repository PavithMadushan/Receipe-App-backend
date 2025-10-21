// File: Services/IRecipeService.cs
using backend.Models.DTOs.Recipe;

namespace backend.Services
{
    public interface IRecipeService
    {
        Task<RecipeDetailDTO?> GetByMealId(string mealId);
        Task<List<RecipeDTO>> GetByCategory(string category);
        Task<List<string>> GetCategories();
    }
}
