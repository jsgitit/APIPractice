using Asp.Versioning;
using CompanyWebApi.Contracts.Converters.V3;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Controllers.Base;
using CompanyWebApi.Persistence.Repositories.Factory;
using CompanyWebApi.Services.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyWebApi.Controllers.V3;

[ApiAuthorization]
[ApiController]
//[ApiVersion("2.1", Deprecated = true)]
[ApiVersion("3.0")]
[Produces("application/json")]
[EnableCors("EnableCORS")]
[ServiceFilter(typeof(ValidModelStateAsyncActionFilter))]
[Route("api/v{version:apiVersion}/[controller]")]
public class EmployeeAddressesController : BaseController<EmployeeAddressesController>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IConverter<EmployeeAddress, EmployeeAddressDto> _employeeAddressToDtoConverter;
    private readonly IConverter<IList<EmployeeAddress>, IList<EmployeeAddressDto>> _employeeAddressToDtoListConverter;
    private readonly IConverter<EmployeeAddressCreateDto, EmployeeAddress> _employeeAddressFromDtoConverter;
    private readonly IConverter<IList<EmployeeAddressUpdateDto>, IList<EmployeeAddress>> _employeeAddressFromUpdateDtoListConverter;

    public EmployeeAddressesController(IRepositoryFactory repositoryFactory,
            IConverter<EmployeeAddress, EmployeeAddressDto> employeeAddressToDtoConverter,
            IConverter<IList<EmployeeAddress>, IList<EmployeeAddressDto>> employeeAddressToDtoListConverter,
            IConverter<EmployeeAddressCreateDto, EmployeeAddress> employeeAddressFromDtoConverter,
            IConverter<IList<EmployeeAddressUpdateDto>, IList<EmployeeAddress>> employeeAddressFromUpdateDtoListConverter)
    {
        _repositoryFactory = repositoryFactory;
        _employeeAddressToDtoConverter = employeeAddressToDtoConverter;
        _employeeAddressToDtoListConverter = employeeAddressToDtoListConverter;
        _employeeAddressFromDtoConverter = employeeAddressFromDtoConverter;
        _employeeAddressFromUpdateDtoListConverter = employeeAddressFromUpdateDtoListConverter;
    }

    /// <summary>
    /// Get an employee address with employee Id and address type Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v3/employeeAddresses/6/1
    ///
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "addressTypeId": 1,
    ///       "address": "Milano, Italy",
    ///       "created": "2024-06-18T17:53:51.9976026",
    ///       "modified": "2024-06-18T17:53:51.9976028"
    ///     }
    /// </remarks>
    /// <param name="employeeId" example="6">Employee Id</param>
    /// <param name="addressTypeId" example="1">Address Type Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeAddressDto), Description = "Return employee address")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee address was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpGet("{employeeId:int}/{addressTypeId:AddressType}", Name = "GetEmployeeAddressByIdsV3")]
    public async Task<ActionResult<EmployeeAddressDto>> GetAsync(int employeeId, AddressType addressTypeId, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAsync));
        var employeeAddress = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressAsync(employeeId, addressTypeId).ConfigureAwait(false);
        if (employeeAddress == null)
        {
            return NotFound(new { message = "The employee address was not found" });
        }
        var employeeAddressDto = _employeeAddressToDtoConverter.Convert(employeeAddress);
        return Ok(employeeAddressDto);
    }

    /// <summary>
    /// Add a new employee address
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "employeeId": 6,
    ///       "addressTypeId": 2,
    ///       "address": "123 Mailing Address street, SomeTown, WA 96800"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "addressTypeId": 2,
    ///       "address": "123 Mailing Address street, SomeTown, WA 96800",
    ///       "created": "2024-06-18T17:53:51.9976026",
    ///       "modified": "2024-06-18T17:53:51.9976028"
    ///     }
    /// </remarks>
    /// <param name="address">EmployeeAddressCreateDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EmployeeAddressDto), Description = "Returns a new employee address")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Employee Address was not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Employee and Address Type already exists")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpPost("create", Name = "CreateEmployeeAddressV3")]
    public async Task<IActionResult> CreateAsync([FromBody] EmployeeAddressCreateDto address, ApiVersion version)
    {
        Logger.LogDebug(nameof(CreateAsync));
        var newEmployeeAddress = _employeeAddressFromDtoConverter.Convert(address);
        if (!await _repositoryFactory.EmployeeRepository.ExistsAsync(e => e.EmployeeId == address.EmployeeId))
        {
            return NotFound(new { message = $"The Employee with id {address.EmployeeId} was not found" });
        }
        if (await _repositoryFactory.EmployeeAddressRepository.ExistsAsync(ea => ea.EmployeeId == address.EmployeeId && ea.AddressTypeId == address.AddressTypeId).ConfigureAwait(false))
        {
            return BadRequest(new { message = $"The Employee with id {address.EmployeeId} and Address Type Id {address.AddressTypeId} already exists" });
        }

        var repoEmployeeAddress = await _repositoryFactory.EmployeeAddressRepository.AddEmployeeAddressAsync(newEmployeeAddress).ConfigureAwait(false);
        var result = _employeeAddressToDtoConverter.Convert(repoEmployeeAddress);
        var createdResult = new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status201Created
        };
        return createdResult;
    }

    /// <summary>
    /// Deletes an employee address given an EmployeeId and AddressTypeID
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v3/employees/6/2
    ///
    /// Sample response body:
    ///     
    ///    Code 200 Success
    /// 
    /// </remarks>
    /// <param name="employeeId" example="6">Employee Id</param>
    /// <param name="addressTypeId" example="2">Address Type Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Employee address was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No employee / address type was found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpDelete("{employeeId:int}/{addressTypeId:AddressType}", Name = "DeleteEmployeeAddressByIdsV3")]
    public async Task<IActionResult> DeleteAsync(int employeeId, AddressType addressTypeId, ApiVersion version)
    {
        Logger.LogDebug(nameof(DeleteAsync));
        var employeeAddress = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressAsync(employeeId, addressTypeId).ConfigureAwait(false);
        if (employeeAddress == null)
        {
            return NotFound(new { message = "No employee / address type was found" });
        }
        _repositoryFactory.EmployeeAddressRepository.Remove(employeeAddress);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return Ok();
    }

    /// <summary>
    /// Gets all available employee addresses for an employee
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v3/employeeAddress/6
    ///
    /// Sample response body:
    ///
    ///     [
    ///       {
    ///         "employeeId": 6,
    ///         "addressTypeId" : 0,
    ///         "address": "unknown address",
    ///         "created": "2024-06-18T17:53:51.9976026",
    ///         "modified": "2024-06-18T17:53:51.9976028"
    ///       },
    ///       {
    ///         "employeeId": 6,
    ///         "addressTypeId" : 1,
    ///         "address": "Work address",
    ///         "created": "2024-06-18T17:53:51.9976026",
    ///         "modified": "2024-06-18T17:53:51.9976028"
    ///       },
    ///       {
    ///         "employeeId": 6,
    ///         "addressTypeId" : 2,
    ///         "address": "Mailing address",
    ///         "created": "2024-06-18T17:53:51.9976026",
    ///         "modified": "2024-06-18T17:53:51.9976028"
    ///       },
    ///       {
    ///         "employeeId": 6,
    ///         "addressTypeId" : 3,
    ///         "address": "Residential address",
    ///         "created": "2024-06-18T17:53:51.9976026",
    ///         "modified": "2024-06-18T17:53:51.9976028"
    ///       }
    ///     ]
    /// </remarks>
    /// <param name="employeeId">Employee Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeAddressDto>), Description = "Return list of all employee's addresses")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee has no addresses")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpGet("{employeeId:int}", Name = "GetAllEmployeesAddressesV3")]
    public async Task<ActionResult<IEnumerable<EmployeeAddressDto>>> GetAllEmployeesAddressesAsync(int employeeId, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAllEmployeesAddressesAsync));
        var employeesAddresses = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressesAsync(ea => ea.EmployeeId == employeeId).ConfigureAwait(false);
        if (!employeesAddresses.Any())
        {
            return NotFound(new { message = "The employee has no addresses" });
        }
        var employeeAddressDto = _employeeAddressToDtoListConverter.Convert(employeesAddresses);
        return Ok(employeeAddressDto);
    }

    /// <summary>
    /// Update a single employee address
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "employeeId": 6,
    ///       "addressTypeId": 1,
    ///       "address": "updated address"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "addressTypeId": 1,
    ///       "address": "updated address",
    ///       "created": "2024-06-18T17:53:51.9976026",
    ///       "modified": "2024-06-18T17:53:51.9976028"
    ///     }
    /// </remarks>
    /// <param name="employeeAddress"><see cref="EmployeeAddressUpdateDto"/></param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeAddressDto), Description = "Return updated employee address")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee / address id combination was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpPut("update", Name = "UpdateEmployeeAddressV3")]
    public async Task<ActionResult<EmployeeAddressDto>> UpdateEmployeeAddressAsync([FromBody] EmployeeAddressUpdateDto employeeAddress, ApiVersion version)
    {
        Logger.LogDebug(nameof(UpdateEmployeeAddressAsync));
        var repoEmployeeAddress = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressAsync(employeeAddress.EmployeeId, employeeAddress.AddressTypeId).ConfigureAwait(false);
        if (repoEmployeeAddress == null)
        {
            return NotFound(new { message = "The employee address was not found" });
        }

        repoEmployeeAddress.Address = employeeAddress.Address;

        await _repositoryFactory.EmployeeAddressRepository.UpdateAsync(repoEmployeeAddress);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        var employeeAddressDto = _employeeAddressToDtoConverter.Convert(repoEmployeeAddress);
        return Ok(employeeAddressDto);
    }

    /// <summary>
    /// Update or Insert a batch of employee addresses
    /// (No Deletes will occur)
    /// </summary>
    /// <remarks>
    /// Sample request body:
    /// 
    ///     [
    ///       {
    ///         "employeeId": 5,
    ///         "addressTypeId": 1,
    ///         "address": "updated address for employee 5"
    ///       },
    ///       {
    ///         "employeeId": 5,
    ///         "addressTypeId": 2,
    ///         "address": "new address for employee 5"
    ///       },
    ///       {
    ///         "employeeId": 6,
    ///         "addressTypeId": 1,
    ///         "address": "updated address"
    ///       }
    ///     ]
    ///     
    /// Sample response body:
    ///     
    ///     Code 204 No Content
    ///     
    /// </remarks>
    /// <param name="employeeAddress"><see cref="EmployeeAddressUpdateDto"/></param>
    [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No Content")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee id was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpPut("upsertBatch", Name = "UpsertEmployeeAddressesV3")]
    public async Task<IActionResult> UpsertEmployeeAddressBatchAsync([FromBody] IList<EmployeeAddressUpdateDto> employeeAddresses)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Note: This is an expensive EmployeeId validation - no repo functionality for batch Id validation yet
        foreach (var address in employeeAddresses)
        {

            if (!await _repositoryFactory.EmployeeRepository.ExistsAsync(e => e.EmployeeId == address.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", $"Employee ID {address.EmployeeId} does not exist.");
                Logger.LogWarning("Attempted to upsert addresses for non-existent employee: {address.EmployeeId}", address.EmployeeId);
            }
        }

        if (!ModelState.IsValid)
        {
            return NotFound(ModelState);
        }

        var newEmployeeAddresses = _employeeAddressFromUpdateDtoListConverter
            .Convert(employeeAddresses);

        await _repositoryFactory.EmployeeAddressRepository
            .UpsertEmployeeAddressesAsync(newEmployeeAddresses);

        return NoContent();
    }
}
