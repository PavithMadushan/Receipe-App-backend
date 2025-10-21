// File: AutoMapper/MapperProfile.cs
using AutoMapper;
using backend.Models.DTOs;
using backend.Models.DTOs.Recipe;
using backend.Models.Entities;

namespace backend.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //CreateMap<AddRequestDTO, Product>();
            //CreateMap<UpdateRequestDTO, Product>();
            //CreateMap<Product, ResponseDTO>();
            CreateMap<RegisterDTO, User>();

            // NEW: FavoriteRecipe -> FavoriteRecipeDTO
            CreateMap<FavoriteRecipe, FavoriteRecipeDTO>()
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
