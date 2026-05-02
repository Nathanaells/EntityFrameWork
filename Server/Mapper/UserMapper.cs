using AutoMapper;
using Implemented_MVC.DTOs;


namespace Implemented_MVC.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<RegisterDTO, User>()
        .ForMember(destination => destination.UserName, opt => opt.MapFrom(src => src.Email))
        .ForMember(destination => destination.DisplayName, opt => opt.MapFrom(src => src.Username));


        CreateMap<User, RegisterResponseDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.DisplayName))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}