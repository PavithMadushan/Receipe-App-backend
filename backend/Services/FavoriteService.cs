// File: Services/FavoriteService.cs
using AutoMapper;
using backend.Models.DTOs;
using backend.Models.DTOs.Recipe;
using backend.Models.Entities;
using backend.Repositories;

namespace backend.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepo _repo;
        private readonly IMapper _mapper;
        private readonly IRecipeService _recipeService;

        public FavoriteService(IFavoriteRepo repo, IMapper mapper, IRecipeService recipeService)
        {
            _repo = repo;
            _mapper = mapper;
            _recipeService = recipeService;
        }

        public async Task<Response> AddFavorite(int userId, AddFavoriteDTO addFavoriteDTO)
        {
            var exists = await _repo.ExistsAsync(userId, addFavoriteDTO.MealId);
            if (exists) return new Response(false, "Already added");

            var fav = new FavoriteRecipe
            {
                UserId = userId,
                MealId = addFavoriteDTO.MealId
            };

            await _repo.AddFavoriteAsync(fav);
            return new Response(true, "Added to favorites");
        }

        public async Task<List<FavoriteRecipeDTO>> GetFavorites(int userId)
        {
            var favorites = await _repo.GetFavoritesByUserAsync(userId);
            var result = new List<FavoriteRecipeDTO>();

            foreach (var f in favorites)
            {
                var detail = await _recipeService.GetByMealId(f.MealId);

                var dto = new FavoriteRecipeDTO
                {
                    Id = f.Id,
                    MealId = f.MealId,
                    AddedAt = f.CreatedAt,
                    Title = detail?.Title ?? "Unknown",
                    Category = detail?.Category ?? "N/A",
                    Thumbnail = detail?.Thumbnail ?? ""
                };

                result.Add(dto);
            }

            return result;
        }

        // IMPLEMENTATION MATCHING THE INTERFACE: remove by MealId
        public async Task<Response> RemoveFavorite(int userId, string mealId)
        {
            var fav = await _repo.GetByUserAndMealIdAsync(userId, mealId);
            if (fav == null) return new Response(false, "Favorite not found");

            if (fav.UserId != userId) // defensive check, though query already filtered by userId
                return new Response(false, "Unauthorized");

            await _repo.DeleteFavoriteAsync(fav);
            return new Response(true, "Removed from favorites");
        }
    }
}
