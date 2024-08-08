using System;
using System.Collections.Generic;

namespace CompanyWebApi.Contracts.Dto.V4;
public class CompanyFullDto
{
    /// <summary>
    /// Company Full Data Transfer Object
    /// </summary>

    public int CompanyId { get; set; }
    public string Name { get; set; }
    public IEnumerable<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
