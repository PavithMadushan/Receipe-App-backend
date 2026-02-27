// File: Services/IFavoriteService.cs
using backend.Models.DTOs;
using backend.Models.DTOs.Recipe;

namespace backend.Services
{
    public interface IFavoriteService
    {
        Task<Response> AddFavorite(int userId, AddFavoriteDTO addFavoriteDTO);
        Task<List<FavoriteRecipeDTO>> GetFavorites(int userId);

        // UPDATED: remove by MealId (string) instead of favorite database id (int)
        Task<Response> RemoveFavorite(int userId, string mealId);
    }
}
