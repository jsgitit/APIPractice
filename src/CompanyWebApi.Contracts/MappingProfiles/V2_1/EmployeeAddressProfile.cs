using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.MappingProfiles.V2_1;
public class EmployeeAddressProfile : Profile
{
    public EmployeeAddressProfile()
    {
        CreateMap<EmployeeAddress, EmployeeAddressDto>();
        CreateMap<EmployeeAddressCreateDto, EmployeeAddress>();
    }
}
