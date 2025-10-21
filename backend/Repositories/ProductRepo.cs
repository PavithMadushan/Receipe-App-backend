//using AutoMapper;
//using backend.Data;
//using backend.Models.DTOs;
//using backend.Models.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace backend.Repositories
//{
//    public class ProductRepo(IMapper mapper, AppDbContext appDbContext) : IProductRepo
//    {
//        public async Task<Response> Add(AddRequestDTO request)
//        {
//            appDbContext.Products.Add(mapper.Map<Product>(request));
//            await appDbContext.SaveChangesAsync();
//            return new Response(true, "Saved");
//        }


//        public async Task<List<ResponseDTO>> GetAll()=>
//            mapper.Map<List<ResponseDTO>>(await appDbContext.Products.ToListAsync());
        

//        public async Task<ResponseDTO> GetById(int id)=>
//            mapper.Map<ResponseDTO>(await appDbContext.Products.FindAsync(id));


//        public async Task<Response> Update(UpdateRequestDTO request)
//        {
//            appDbContext.Products.Update(mapper.Map<Product>(request));
//            await appDbContext.SaveChangesAsync();
//            return new Response(true, "Updated");
//        }


//        public async Task<Response> Delete(int id)
//        {
//            appDbContext.Products.Remove(await appDbContext.Products.FindAsync(id));
//            await appDbContext.SaveChangesAsync();
//            return new Response(true, "Delete");
//        }

        







//        Task<Response> IProductRepo.Update(UpdateRequestDTO request)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
