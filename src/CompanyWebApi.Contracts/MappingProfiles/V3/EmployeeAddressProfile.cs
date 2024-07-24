using AutoMapper;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V3
{
    public class EmployeeAddressProfile : Profile
    {
        public EmployeeAddressProfile()
        {
            CreateMap<EmployeeAddress, EmployeeAddressDto>().ReverseMap();
            CreateMap<EmployeeAddress, EmployeeAddressUpdateDto>().ReverseMap();
            CreateMap<EmployeeAddress, EmployeeAddressCreateDto>().ReverseMap();
        }
    }
}
