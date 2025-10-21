// File: Services/RecipeService.cs
using System.Net.Http.Json;
using backend.Models.DTOs.Recipe;

namespace backend.Services
{
    // Simple wrapper to call TheMealDB public endpoints
    public class RecipeService : IRecipeService
    {
        private readonly IHttpClientFactory _httpFactory;
        private const string BASE = "https://www.themealdb.com/api/json/v1/1";

        public RecipeService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<RecipeDetailDTO?> GetByMealId(string mealId)
        {
            var client = _httpFactory.CreateClient();
            var url = $"{BASE}/lookup.php?i={mealId}";
            var resp = await client.GetFromJsonAsync<MealLookupResponse>(url);
            var meal = resp?.meals?.FirstOrDefault();
            if (meal == null) return null;

            return new RecipeDetailDTO
            {
                MealId = meal.idMeal,
                Title = meal.strMeal,
                Category = meal.strCategory,
                Thumbnail = meal.strMealThumb,
                Instructions = meal.strInstructions,
                YoutubeLink = meal.strYoutube,
                //SourceUrl = meal.strSource
            };
        }

        public async Task<List<RecipeDTO>> GetByCategory(string category)
        {
            var client = _httpFactory.CreateClient();
            var url = $"{BASE}/filter.php?c={Uri.EscapeDataString(category)}";
            var resp = await client.GetFromJsonAsync<MealListResponse>(url);

            var list = new List<RecipeDTO>();
            if (resp?.meals == null) return list;

            list.AddRange(resp.meals.Select(m => new RecipeDTO
            {
                MealId = m.idMeal,
                Title = m.strMeal,
                Thumbnail = m.strMealThumb,
                Category = category
            }));

            return list;
        }

        public async Task<List<string>> GetCategories()
        {
            var client = _httpFactory.CreateClient();
            var url = $"{BASE}/categories.php";
            var resp = await client.GetFromJsonAsync<CategoryListResponse>(url);
            if (resp?.categories == null) return new List<string>();
            return resp.categories.Select(c => c.strCategory).ToList();
        }

        // --- Response DTOs for TheMealDB JSON (internal models)
        private class MealLookupResponse { public List<MealDetail>? meals { get; set; } }
        private class MealListResponse { public List<MealListItem>? meals { get; set; } }
        private class CategoryListResponse { public List<CategoryItem>? categories { get; set; } }

        private class MealDetail
        {
            public string idMeal { get; set; } = "";
            public string strMeal { get; set; } = "";
            public string strCategory { get; set; } = "";
            public string strMealThumb { get; set; } = "";
            public string strInstructions { get; set; } = "";
            public string strYoutube { get; set; } = "";
            public string strSource { get; set; } = "";
        }

        private class MealListItem
        {
            public string idMeal { get; set; } = "";
            public string strMeal { get; set; } = "";
            public string strMealThumb { get; set; } = "";
        }

        private class CategoryItem
        {
            public string idCategory { get; set; } = "";
            public string strCategory { get; set; } = "";
        }
    }
}
