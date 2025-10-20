using backend.AutoMapper;
using backend.Data;
using backend.Models.DTOs;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
