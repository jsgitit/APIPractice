using AutoMapper;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.MappingProfiles.V3
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src =>
                    src.EmployeeAddresses != null ? src.EmployeeAddresses.Select(a => new EmployeeAddressDto
                    {
                        EmployeeId = a.EmployeeId,
                        AddressTypeId = a.AddressTypeId,
                        Address = a.Address,
                        Created = a.Created,
                        Modified = a.Modified
                    }).ToList() : new List<EmployeeAddressDto>()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.Username : string.Empty))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src =>
                    src.Company != null ? src.Company.Name : string.Empty))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>
                    src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age));

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
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));
        }
    }
}
