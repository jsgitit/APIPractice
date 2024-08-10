using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Dto.V4;
public class DepartmentFullListDto
{
    public IEnumerable<DepartmentFullDto> Departments { get; set; } = Enumerable.Empty<DepartmentFullDto>();
}
