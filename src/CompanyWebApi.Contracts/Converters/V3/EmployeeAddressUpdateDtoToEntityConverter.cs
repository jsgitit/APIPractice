using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Converters.V3;
public class EmployeeAddressUpdateDtoToEntityConverter : IConverter<EmployeeAddressUpdateDto, EmployeeAddress>, IConverter<IList<EmployeeAddressUpdateDto>, IList<EmployeeAddress>>
{
    private readonly ILogger<EmployeeAddressUpdateDtoToEntityConverter> _logger;

    public EmployeeAddressUpdateDtoToEntityConverter(ILogger<EmployeeAddressUpdateDtoToEntityConverter> logger)
    {
        _logger = logger;
    }
    public EmployeeAddress Convert(EmployeeAddressUpdateDto employeeAddressDto)
    {
        _logger.LogDebug("Convert");
        return new EmployeeAddress
        {
            EmployeeId = employeeAddressDto.EmployeeId,
            AddressTypeId = employeeAddressDto.AddressTypeId,
            Address = employeeAddressDto.Address
        };
    }

    public IList<EmployeeAddress> Convert(IList<EmployeeAddressUpdateDto> employeeAddressDtos)
    {
        _logger.LogDebug("ConvertList");
        return employeeAddressDtos.Select(Convert).ToList();
    }
}
