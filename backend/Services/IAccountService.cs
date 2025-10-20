using backend.Models.DTOs;

namespace backend.Services
{
    public interface IAccountService
    {
        Task<Response> Register(RegisterDTO registerDTO);
        Task<LoginResponse> Login(LoginDTO loginDTO);
    }
}
