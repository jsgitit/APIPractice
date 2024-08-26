using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V4;

public class EmployeeAddressProfile : Profile
{
    public EmployeeAddressProfile()
    {
        CreateMap<EmployeeAddress, EmployeeAddressDto>().ReverseMap();
        CreateMap<EmployeeAddress, EmployeeAddressUpdateDto>().ReverseMap();
        CreateMap<EmployeeAddress, EmployeeAddressCreateDto>().ReverseMap();
    }
}
