using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Dto.V4;
/// <summary>
/// Employee full details list data transfer object
/// </summary>
public class EmployeeFullListDto
{
    public IEnumerable<EmployeeFullDto> Employees { get; set; } = Enumerable.Empty<EmployeeFullDto>();
}
