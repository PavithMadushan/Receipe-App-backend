using backend.Models.DTOs;
using backend.Repositories;

namespace backend.Services
{
    public class ProductService(IProductRepo product) : IProductServices
    {
        public async Task<Response> Add(AddRequestDTO request)=>await product.Add(request);

        public async Task<Response> Delete(int id) => await product.Delete(id);

        public async Task<List<ResponseDTO>> GetAll() => await product.GetAll();    

        public async Task<ResponseDTO> GetById(int id)=>await product.GetById(id);

        public async Task<Response> Update(UpdateRequestDTO request)=>await product.Update(request);
    }
}
