using backend.AutoMapper;
using backend.Data;
using backend.Models.DTOs;
using backend.Models.DTOs.Recipe;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient for RecipeService
builder.Services.AddHttpClient();

// Configure SQL Server connection
builder.Services.AddDbContext<AppDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

// Dependency Injection
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductServices, ProductService>();

// AutoMapper configuration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});

builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Register new repos & services
builder.Services.AddScoped<IFavoriteRepo, FavoriteRepo>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();



// JWT Authentication configuration
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"], // ❗ Removed extra space
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// ✅ Correct Swagger configuration block
builder.Services.AddSwaggerGen(swagger =>
{
    // Basic Info
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 8 Web API",
        Description = "Authentication using JWT"
    });

    // Enable JWT Authorization in Swagger
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔐 Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();



app.MapPost("/register", async (RegisterDTO register, IAccountService account) =>
{
return Results.Ok(await account.Register(register));
}).AllowAnonymous();

app.MapPost("/login", async (LoginDTO login, IAccountService account) =>
{
    return Results.Ok(await account.Login(login));
}).AllowAnonymous();


// Minimal APIs
app.MapGet("/GetProducts", async (IProductServices productService) =>
{
    return Results.Ok(await productService.GetAll());
}).RequireAuthorization();

app.MapGet("/GetProduct/{id:int}", async (IProductServices productService, int id) =>
{
    return Results.Ok(await productService.GetById(id));
}).RequireAuthorization();

app.MapPost("/AddProduct", async (AddRequestDTO request, IProductServices productService) =>
{
    return Results.Ok(await productService.Add(request));
}).RequireAuthorization();

app.MapPut("/updateProduct", async (UpdateRequestDTO request, IProductServices productService) =>
{
    return Results.Ok(await productService.Update(request));
}).RequireAuthorization();

app.MapGet("/deleteProduct/{id:int}", async (IProductServices productService, int id) =>
{
    return Results.Ok(await productService.Delete(id));
}).RequireAuthorization();



int? GetUserIdFromClaims(ClaimsPrincipal user)
{
    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (idClaim == null) return null;
    if (int.TryParse(idClaim.Value, out var id)) return id;
    return null;
}

// Recipes: get categories
app.MapGet("/recipes/categories", async (IRecipeService recipeService) =>
{
    var categories = await recipeService.GetCategories();
    return Results.Ok(categories);
}).AllowAnonymous();

// Recipes: get by category
app.MapGet("/recipes/by-category/{category}", async (IRecipeService recipeService, string category) =>
{
    var recipes = await recipeService.GetByCategory(category);
    return Results.Ok(recipes);
}).AllowAnonymous();

// Recipes: get detail by meal id
app.MapGet("/recipes/{mealId}", async (IRecipeService recipeService, string mealId) =>
{
    var detail = await recipeService.GetByMealId(mealId);
    if (detail == null) return Results.NotFound();
    return Results.Ok(detail);
}).AllowAnonymous();


// Favorites: add favorite (authenticated)
app.MapPost("/favorites", async (AddFavoriteDTO addFavoriteDTO, IFavoriteService favService, HttpContext http) =>
{
    var userId = GetUserIdFromClaims(http.User);
    if (userId == null) return Results.Unauthorized();
    var res = await favService.AddFavorite(userId.Value, addFavoriteDTO);
    return Results.Ok(res);
}).RequireAuthorization();

// Favorites: get current user's favorites
app.MapGet("/favorites", async (IFavoriteService favService, HttpContext http) =>
{
    var userId = GetUserIdFromClaims(http.User);
    if (userId == null) return Results.Unauthorized();
    var list = await favService.GetFavorites(userId.Value);
    return Results.Ok(list);
}).RequireAuthorization();

// Favorites: delete favorite by id
app.MapDelete("/favorites/{id:int}", async (int id, IFavoriteService favService, HttpContext http) =>
{
    var userId = GetUserIdFromClaims(http.User);
    if (userId == null) return Results.Unauthorized();
    var res = await favService.RemoveFavorite(userId.Value, id);
    return Results.Ok(res);
}).RequireAuthorization();



app.Run();
