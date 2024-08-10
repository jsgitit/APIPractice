using System;
using System.Collections.Generic;

namespace CompanyWebApi.Contracts.Dto.V4;
public class DepartmentFullDto
{
    public int DepartmentId { get; set; }
    public string Name { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public IEnumerable<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
