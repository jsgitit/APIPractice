using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;
using System.Linq;

namespace CompanyWebApi.Contracts.MappingProfiles.V2_1;
public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src =>
                src.Employees.Select(e =>
                    $"{e.FirstName} {e.LastName}, Address: {GetWorkAddress(e)}, Department: {GetDepartmentName(e)}, Username: {GetUsername(e)}"
            ).ToList()));
    }

    private static string GetWorkAddress(Employee e)
    {
        var workAddress = e.EmployeeAddresses.FirstOrDefault(a => a.AddressTypeId == AddressType.Work);
        return workAddress != null ? workAddress.Address : string.Empty;
    }

    private static string GetUsername(Employee e)
    {
        return e.User != null ? e.User.Username : string.Empty;
    }

    private static string GetDepartmentName(Employee e)
    {
        // Assuming there's a way to get the department name; adjust as needed
        return e.Department.Name ?? string.Empty;
    }
}
