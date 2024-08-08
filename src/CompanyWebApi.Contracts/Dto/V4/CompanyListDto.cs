using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Dto.V4;
public class CompanyListDto
{
    public IEnumerable<CompanyDto> Companies { get; set; } = Enumerable.Empty<CompanyDto>();
}
