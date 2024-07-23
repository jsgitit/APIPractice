using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V2_1;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserAuthenticateDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName));
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName));
    }
}
