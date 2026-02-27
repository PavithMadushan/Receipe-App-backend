// File: Repositories/FavoriteRepo.cs
using backend.Data;
using backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class FavoriteRepo : IFavoriteRepo
    {
        private readonly AppDbContext _db;
        public FavoriteRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<FavoriteRecipe> AddFavoriteAsync(FavoriteRecipe favorite)
        {
            _db.FavoriteRecipes.Add(favorite);
            await _db.SaveChangesAsync();
            return favorite;
        }

        public async Task<List<FavoriteRecipe>> GetFavoritesByUserAsync(int userId)
        {
            return await _db.FavoriteRecipes
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<FavoriteRecipe?> GetFavoriteByIdAsync(int id)
        {
            return await _db.FavoriteRecipes.FindAsync(id);
        }

        public async Task DeleteFavoriteAsync(FavoriteRecipe favorite)
        {
            _db.FavoriteRecipes.Remove(favorite);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId, string mealId)
        {
            return await _db.FavoriteRecipes.AnyAsync(f => f.UserId == userId && f.MealId == mealId);
        }

        // ✅ NEW METHOD — used for deletion by MealId
        public async Task<FavoriteRecipe?> GetByUserAndMealIdAsync(int userId, string mealId)
        {
            return await _db.FavoriteRecipes
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MealId == mealId);
        }
    }
}
