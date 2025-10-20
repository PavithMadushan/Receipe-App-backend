using backend.Models.DTOs;

namespace backend.Repositories
{
    public interface IProductRepo
    {
        Task<Response> Add(AddRequestDTO request);

        Task<Response> Update(UpdateRequestDTO request);

        Task<List<ResponseDTO>> GetAll();

        Task<ResponseDTO> GetById(int id);

        Task<Response> Delete(int id);
    }
}
