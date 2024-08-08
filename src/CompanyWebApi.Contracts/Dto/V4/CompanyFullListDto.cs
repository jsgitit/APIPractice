using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Dto.V4;
public class CompanyFullListDto
{
    public IEnumerable<CompanyFullDto> Companies { get; set; } = Enumerable.Empty<CompanyFullDto>();
}
