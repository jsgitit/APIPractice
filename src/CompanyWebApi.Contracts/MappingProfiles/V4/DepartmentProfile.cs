using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V4;
public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>();
        CreateMap<Department, DepartmentFullDto>();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Department.Company.Name))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Department.Company.CompanyId))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.DepartmentId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : string.Empty))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.EmployeeAddresses));

        CreateMap<EmployeeAddress, EmployeeAddressDto>();
    }
}
