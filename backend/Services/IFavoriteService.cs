// File: Services/IFavoriteService.cs
using backend.Models.DTOs;
using backend.Models.DTOs.Recipe;

namespace backend.Services
{
    public interface IFavoriteService
    {
        Task<Response> AddFavorite(int userId, AddFavoriteDTO addFavoriteDTO);
        Task<List<FavoriteRecipeDTO>> GetFavorites(int userId);
        Task<Response> RemoveFavorite(int userId, int favoriteId);
    }
}
