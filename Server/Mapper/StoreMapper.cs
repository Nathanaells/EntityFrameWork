using AutoMapper;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Mapper;

public class StoreMapper : Profile
{
    public StoreMapper()
    {
        CreateMap<StoreDTO, Store>();

        CreateMap<UpdateStoreDTO, Store>();

        CreateMap<Store, StoreResponseDTO>();
    }

}