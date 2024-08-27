using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V4;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserAuthenticateDto>()
            .ForMember(dest => dest.FirstName,
            opt => opt.MapFrom(src => src.Employee.FirstName)) // nested property
            .ForMember(dest => dest.LastName,
            opt => opt.MapFrom(src => src.Employee.LastName)); // nested property

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName));

        CreateMap<UserCreateDto, User>();
    }
}
