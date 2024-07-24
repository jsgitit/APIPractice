using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.MappingProfiles.V2_1;
public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                GetAddress(src) ?? string.Empty))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src =>
                src.User != null ? src.User.Username ?? string.Empty : string.Empty))
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src =>
                src.Company != null ? src.Company.Name ?? string.Empty : string.Empty))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>
                src.Department != null ? src.Department.Name ?? string.Empty : string.Empty))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty));

        CreateMap<EmployeeCreateDto, Employee>()
            .ForMember(dest => dest.EmployeeAddresses, opt => opt.MapFrom(src =>
                new List<EmployeeAddress>
                {
                    new EmployeeAddress
                    {
                        Address = src.Address ?? string.Empty,
                        AddressTypeId = AddressType.Work
                    }
                }))
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
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));
    }

    private string GetAddress(Employee src)
    {
        // Check for null and empty lists
        if (src.EmployeeAddresses != null && src.EmployeeAddresses.Any())
        {
            var workAddress = src.EmployeeAddresses
                .FirstOrDefault(a => a.AddressTypeId == AddressType.Work);
            return workAddress != null ? workAddress.Address ?? string.Empty : string.Empty;
        }
        return string.Empty;
    }
}
