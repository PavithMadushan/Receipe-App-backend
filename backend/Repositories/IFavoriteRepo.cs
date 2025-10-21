// File: Repositories/IFavoriteRepo.cs
using backend.Models.DTOs.Recipe;
using backend.Models.Entities;

namespace backend.Repositories
{
    public interface IFavoriteRepo
    {
        Task<FavoriteRecipe> AddFavoriteAsync(FavoriteRecipe favorite);
        Task<List<FavoriteRecipe>> GetFavoritesByUserAsync(int userId);
        Task<FavoriteRecipe?> GetFavoriteByIdAsync(int id);
        Task DeleteFavoriteAsync(FavoriteRecipe favorite);
        Task<bool> ExistsAsync(int userId, string mealId);
    }
}
