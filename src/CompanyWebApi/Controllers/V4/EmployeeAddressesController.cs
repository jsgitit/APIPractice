using Asp.Versioning;
using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
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

namespace CompanyWebApi.Controllers.V4;

[ApiAuthorization]
[ApiController]
[ApiVersion("4.0")]
[Produces("application/json")]
[EnableCors("EnableCORS")]
[ServiceFilter(typeof(ValidModelStateAsyncActionFilter))]
[Route("api/v{version:apiVersion}/[controller]")]
public class EmployeeAddressesController : BaseController<EmployeeAddressesController>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;

    public EmployeeAddressesController(IRepositoryFactory repositoryFactory,
        IMapper mapper)
    {
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
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
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EmployeeAddressDto), Description = "Returns a new employee address")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Employee Address was not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Employee and Address Type already exists")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpPost(Name = nameof(CreateAsync))]
    public async Task<IActionResult> CreateAsync([FromBody] EmployeeAddressCreateDto address)
    {
        Logger.LogDebug(nameof(CreateAsync));
        var newEmployeeAddress = _mapper.Map<EmployeeAddress>(address);
        if (!await _repositoryFactory.EmployeeRepository.ExistsAsync(e => e.EmployeeId == address.EmployeeId))
        {
            return NotFound(new { message = $"The Employee with id {address.EmployeeId} was not found" });
        }
        if (await _repositoryFactory.EmployeeAddressRepository.ExistsAsync(ea => ea.EmployeeId == address.EmployeeId && ea.AddressTypeId == address.AddressTypeId).ConfigureAwait(false))
        {
            return BadRequest(new { message = $"The Employee with id {address.EmployeeId} and Address Type Id {address.AddressTypeId} already exists" });
        }

        var repoEmployeeAddress = await _repositoryFactory.EmployeeAddressRepository.AddEmployeeAddressAsync(newEmployeeAddress).ConfigureAwait(false);
        var employeeAddressDto = _mapper.Map<EmployeeAddressDto>(repoEmployeeAddress);

        // TODO: Fix inconsistent route, int or AddressType? CreatedAtRoute wasn't working well with AddressType for some reason
        return CreatedAtRoute(nameof(GetAsync), new { employeeId = employeeAddressDto.EmployeeId, addressTypeId = (int)employeeAddressDto.AddressTypeId }, employeeAddressDto);
    }

    /// <summary>
    /// Get an employee address with employee Id and address type Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/employeeAddresses/6/1
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
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeAddressDto), Description = "Return employee address")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee address was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpGet("{employeeId:int}/{addressTypeId:int}", Name = nameof(GetAsync))]
    public async Task<ActionResult<EmployeeAddressDto>> GetAsync(int employeeId, AddressType addressTypeId)
    {
        Logger.LogDebug(nameof(GetAsync));
        var employeeAddress = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressAsync(employeeId, addressTypeId).ConfigureAwait(false);
        if (employeeAddress == null)
        {
            return NotFound(new { message = "The employee address was not found" });
        }
        var employeeAddressDto = _mapper.Map<EmployeeAddressDto>(employeeAddress);
        return Ok(employeeAddressDto);
    }

    /// <summary>
    /// Gets all available employee addresses for an employee
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/employeeAddress/6
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
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeAddressDto>), Description = "Return list of all employee's addresses")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee has no addresses")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpGet("{employeeId:int}", Name = nameof(GetAllEmployeesAddressesAsync))]
    public async Task<ActionResult<IEnumerable<EmployeeAddressDto>>> GetAllEmployeesAddressesAsync(int employeeId)
    {
        Logger.LogDebug(nameof(GetAllEmployeesAddressesAsync));
        var employeesAddresses = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressesAsync(ea => ea.EmployeeId == employeeId).ConfigureAwait(false);
        if (!employeesAddresses.Any())
        {
            return NotFound(new { message = "The employee has no addresses" });
        }
        var employeeAddressDto = _mapper.Map<IEnumerable<EmployeeAddressDto>>(employeesAddresses);
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
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeAddressDto), Description = "Return updated employee address")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee / address id combination was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpPut(Name = nameof(UpdateEmployeeAddressAsync))]
    public async Task<ActionResult<EmployeeAddressDto>> UpdateEmployeeAddressAsync([FromBody] EmployeeAddressUpdateDto employeeAddress)
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
        var employeeAddressDto = _mapper.Map<EmployeeAddressDto>(repoEmployeeAddress);
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
    /// <param name="employeeAddresses"><see cref="EmployeeAddressUpdateDto"/></param>
    [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No Content")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee id was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpPut("upsertBatch", Name = nameof(UpsertEmployeeAddressBatchAsync))]
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

        var newEmployeeAddresses = _mapper.Map<IList<EmployeeAddress>>(employeeAddresses);

        await _repositoryFactory.EmployeeAddressRepository
            .UpsertEmployeeAddressesAsync(newEmployeeAddresses);

        return NoContent();
    }
    /// <summary>
    /// Deletes an employee address given an EmployeeId and AddressTypeID
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v4/employees/6/2
    ///
    /// Sample response body:
    ///     
    ///    Code 204 No Content
    /// 
    /// </remarks>
    /// <param name="employeeId" example="6">Employee Id</param>
    /// <param name="addressTypeId" example="2">Address Type Id</param>
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Employee address was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No employee / address type was found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [SwaggerOperation(Tags = new[] { "Employee's Addresses" })]
    [HttpDelete("{employeeId:int}/{addressTypeId:AddressType}", Name = nameof(DeleteAsync))]
    public async Task<IActionResult> DeleteAsync(int employeeId, AddressType addressTypeId)
    {
        Logger.LogDebug(nameof(DeleteAsync));
        var employeeAddress = await _repositoryFactory.EmployeeAddressRepository.GetEmployeeAddressAsync(employeeId, addressTypeId).ConfigureAwait(false);
        if (employeeAddress == null)
        {
            return NotFound(new { message = "No employee / address type was found" });
        }
        _repositoryFactory.EmployeeAddressRepository.Remove(employeeAddress);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return NoContent();
    }
}
