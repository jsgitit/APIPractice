using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Converters.V3;
public class EmployeeAddressFromDtoConverter : IConverter<EmployeeAddressCreateDto, EmployeeAddress>, IConverter<IList<EmployeeAddressCreateDto>, IList<EmployeeAddress>>
{
    private readonly ILogger<EmployeeAddressFromDtoConverter> _logger;

    public EmployeeAddressFromDtoConverter(ILogger<EmployeeAddressFromDtoConverter> logger)
    {
        _logger = logger;
    }

    public EmployeeAddress Convert(EmployeeAddressCreateDto employeeAddress)
    {
        _logger.LogDebug("Convert");

        var employeeAddressDto = new EmployeeAddress
        {
            EmployeeId = employeeAddress.EmployeeId,
            AddressTypeId = employeeAddress.AddressTypeId,
            Address = employeeAddress.Address
        };
        return employeeAddressDto;
    }
    public IList<EmployeeAddress> Convert(IList<EmployeeAddressCreateDto> employeeAddresses)
    {
        _logger.LogDebug("ConvertList");
        return employeeAddresses.Select(Convert).ToList();
    }
}
