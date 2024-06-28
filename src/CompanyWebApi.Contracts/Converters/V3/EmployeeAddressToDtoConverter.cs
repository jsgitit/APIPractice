using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Converters.V3;
public class EmployeeAddressToDtoConverter : IConverter<EmployeeAddress, EmployeeAddressDto>, IConverter<IList<EmployeeAddress>, IList<EmployeeAddressDto>>
{
    private readonly ILogger<EmployeeAddressToDtoConverter> _logger;

    public EmployeeAddressToDtoConverter(ILogger<EmployeeAddressToDtoConverter> logger)
    {
        _logger = logger;
    }

    public EmployeeAddressDto Convert(EmployeeAddress employeeAddress)
    {
        _logger.LogDebug("Convert");
        var employeeAddressDto = new EmployeeAddressDto
        {
            EmployeeId = employeeAddress.EmployeeId,
            AddressTypeId = employeeAddress.AddressTypeId,
            Address = employeeAddress.Address
        };
        return employeeAddressDto;
    }
    public IList<EmployeeAddressDto> Convert(IList<EmployeeAddress> employeeAddresses)
    {
        _logger.LogDebug("ConvertList");
        return employeeAddresses.Select(Convert).ToList();
    }
}
