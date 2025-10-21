using AutoMapper;
using backend.Data;
using backend.Models.DTOs;
using backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        // ✅ Constructor Injection (C# 11 style with parameters converted to fields)
        public AccountRepo(AppDbContext appDbContext, IMapper mapper, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _config = config;
        }

        // ✅ LOGIN METHOD
        public async Task<LoginResponse> Login(LoginDTO loginDTO)
        {
            // Check if user exists
            var user = await FindUserByEmail(loginDTO.Email);
            if (user == null)
                return new LoginResponse(false, null, "User does not exist");

            // ✅ Verify password using BCrypt
            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
            if (!verifyPassword)
                return new LoginResponse(false, null, "Invalid credentials");

            // ✅ Generate JWT token
            string token = GenerateToken(user);
            return new LoginResponse(true, token, null);
        }

        // ✅ REGISTER METHOD
        public async Task<Response> Register(RegisterDTO registerDTO)
        {
            var user = await FindUserByEmail(registerDTO.Email);
            if (user != null)
                return new Response(false, "User already registered");

            // Map DTO to Entity
            var addUser = _mapper.Map<User>(registerDTO);
            addUser.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            _appDbContext.Users.Add(addUser);
            await _appDbContext.SaveChangesAsync();

            return new Response(true, "User registered successfully");
        }

        // ✅ FIND USER BY EMAIL (helper)
        private async Task<User?> FindUserByEmail(string email)
        {
            email = email.ToLower();
            return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);
        }

        // ✅ GENERATE JWT TOKEN
        // File: Repositories/AccountRepo.cs
        // Replace the GenerateToken method implementation with the following:

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // <-- added: user id
        new Claim("Fullname", user.Name),
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
