using backend.Models.DTOs;

namespace backend.Repositories
{
    public interface IAccountRepo
    {
        Task<Response> Register(RegisterDTO registerDTO);
        Task<LoginResponse> Login(LoginDTO loginDTO);
    }
}
