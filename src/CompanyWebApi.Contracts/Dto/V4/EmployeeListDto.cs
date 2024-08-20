using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Dto.V4;
/// <summary>
/// Employee basic details list data transfer object
/// </summary>
public class EmployeeListDto
{
    public IEnumerable<EmployeeDto> Employees { get; set; } = Enumerable.Empty<EmployeeDto>();
}
