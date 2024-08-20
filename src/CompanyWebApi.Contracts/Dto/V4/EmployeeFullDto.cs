using System;
using System.Collections.Generic;

namespace CompanyWebApi.Contracts.Dto.V4;

/// <summary>
/// Employee full details data transfer object
/// </summary>
public class EmployeeFullDto
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public int Age { get; set; }

    public IList<EmployeeAddressDto> Addresses { get; set; }

    public string Username { get; set; }

    public int CompanyId { get; set; }

    public string Company { get; set; }

    public int DepartmentId { get; set; }

    public string Department { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
