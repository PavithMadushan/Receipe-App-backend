// File: Data/AppDbContext.cs
using backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        // NEW: favorite recipes
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }

        // Optional: configure relationships in OnModelCreating if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // allow MealId to have an index (optional but useful)
            modelBuilder.Entity<FavoriteRecipe>()
                .HasIndex(fr => new { fr.UserId, fr.MealId })
                .IsUnique(false); // allow multiple users to favorite same meal
        }
    }
}
