using AutoMapper;
using backend.Models.DTOs;
using backend.Models.Entities;

namespace backend.AutoMapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile() {
            CreateMap<AddRequestDTO, Product>();
            CreateMap<UpdateRequestDTO, Product>();
            CreateMap<Product,ResponseDTO>();
            CreateMap<RegisterDTO, User>();

        }
    }
}
