using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Dto.V4;
public class DepartmentListDto
{
    public IEnumerable<DepartmentDto> Departments { get; set; } = Enumerable.Empty<DepartmentDto>();
}
