using ItemStore.WebApi.Model.DTO;
using AutoMapper;
using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {

            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();

        }
        
    }
}
