using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using System.Linq;

namespace CompanyWebApi.Contracts.MappingProfiles.V4;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.CompanyId))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.DepartmentId));

        CreateMap<Employee, EmployeeFullDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.CompanyId))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.DepartmentId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : string.Empty))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.EmployeeAddresses));

        CreateMap<EmployeeCreateDto, Employee>()
            .ForMember(dest => dest.EmployeeAddresses, opt => opt.MapFrom(src =>
                src.Addresses.Select(a => new EmployeeAddress
                {
                    AddressTypeId = a.AddressTypeId,
                    Address = a.Address
                }).ToList()))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.Username) && !string.IsNullOrEmpty(src.Password)
                    ? new User
                    {
                        Username = src.Username ?? string.Empty,
                        Password = src.Password ?? string.Empty
                    }
                    : null))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore());
        
        CreateMap<EmployeeAddress, EmployeeAddressDto>();

        CreateMap<EmployeeAddressUpdateDto, EmployeeAddress>()
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore());
    }
}
