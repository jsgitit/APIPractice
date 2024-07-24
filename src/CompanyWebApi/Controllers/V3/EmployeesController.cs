using Asp.Versioning;
using AutoMapper;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.Models;
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
[ApiVersion("3.0")]
[Produces("application/json")]
[EnableCors("EnableCORS")]
[ServiceFilter(typeof(ValidModelStateAsyncActionFilter))]
[Route("api/v{version:apiVersion}/[controller]")]
public class EmployeesController : BaseController<EmployeesController>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;

    public EmployeesController(IRepositoryFactory repositoryFactory,
        IMapper mapper)
    {
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a new employee, with multiple address types
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "1971-05-18",
    ///       "companyId": 1,
    ///       "departmentId": 2,
    ///       "addresses": [
    ///         {
    ///           "addressTypeId": 1,
    ///           "address": "Hamilton Street 145/a, 10007 NY"
    ///         },
    ///         {
    ///           "addressTypeId": 2,
    ///           "address": "123 Residential Street"
    ///         },
    ///       ],
    ///       "username": "alanf",
    ///       "password": "tntgroup!129"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 16,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "1971-05-18T00:00:00",
    ///       "age": 50,
    ///       "addresses": [
    ///        {
    ///            "employeeId": 16,
    ///            "addressTypeId": 0,
    ///            "address": "000 Unknown St",
    ///            "created": "2024-07-13T15:40:05.9319142Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///            "employeeId": 16,
    ///            "addressTypeId": 1,
    ///            "address": "123 Work St",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///       ],
    ///       "username": "alanf",
    ///       "companyId": 1,
    ///       "company": "Company One",
    ///       "departmentId": 2,
    ///       "department": "Some Department",
    ///       "created": "2024-07-13T15:40:05.9319504Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="employee">EmployeeCreateDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EmployeeDto), Description = "Returns a new employee")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Company or Department was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPost("create", Name = "CreateEmployeeV3")]
    public async Task<IActionResult> CreateAsync([FromBody] EmployeeCreateDto employee, ApiVersion version)
    {
        Logger.LogDebug(nameof(CreateAsync));
        var newEmployee = _mapper.Map<Employee>(employee);
        if (!await _repositoryFactory.CompanyRepository.ExistsAsync(c => c.CompanyId == employee.CompanyId).ConfigureAwait(false))
        {
            return NotFound(new { message = $"The Company with id {employee.CompanyId} was not found" });
        }
        if (!await _repositoryFactory.DepartmentRepository.ExistsAsync(c => c.DepartmentId == employee.DepartmentId).ConfigureAwait(false))
        {
            return NotFound(new { message = $"The Department with id {employee.DepartmentId} was not found" });
        }

        var repoEmployee = await _repositoryFactory.EmployeeRepository.AddEmployeeAsync(newEmployee).ConfigureAwait(false);
        var result = _mapper.Map<EmployeeDto>(repoEmployee);
        var createdResult = new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status201Created
        };
        return createdResult;
    }

    /// <summary>
    /// Deletes a employee with Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v3/employees/1
    ///
    /// Sample response body:
    ///     
    ///    Code 200 Success
    /// 
    /// </remarks>
    /// <param name="id" example="1">Employee Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Employee was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No employee was found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpDelete("{id:int}", Name = "DeleteEmployeeByIdV3")]
    public async Task<IActionResult> DeleteAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(DeleteAsync));
        var employee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(id).ConfigureAwait(false);
        if (employee == null)
        {
            return NotFound(new { message = "No employee was found" });
        }
        _repositoryFactory.EmployeeRepository.Remove(employee);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return Ok();
    }

    /// <summary>
    /// Gets all employees
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v3/employees/getall
    ///
    /// Sample response body:
    ///
    ///     [
    ///     {
    ///       "employeeId": 6,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "1971-05-18T00:00:00",
    ///       "age": 50,
    ///       "addresses": [
    ///        {
    ///            "employeeId": 6,
    ///            "addressTypeId": 0,
    ///            "address": "000 Unknown St",
    ///            "created": "2024-07-13T15:40:05.9319142Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///            "employeeId": 6,
    ///            "addressTypeId": 1,
    ///            "address": "123 Work St",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///       ],
    ///       "username": "alanf",
    ///       "companyId": 1,
    ///       "company": "Company One",
    ///       "departmentId": 2,
    ///       "department": "Some Department",
    ///       "created": "2024-07-13T15:40:05.9319504Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     },
    ///     {
    ///       "employeeId": 2,
    ///       "firstName": "Mathias",
    ///       "lastName": "Gernold",
    ///       "birthDate": "1997-10-12T00:00:00",
    ///       "age": 50,
    ///       "addresses": [
    ///        {
    ///            "employeeId": 6,
    ///            "addressTypeId": 0,
    ///            "address": "000 Unknown St",
    ///            "created": "2024-07-13T15:40:05.9319142Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///            "employeeId": 6,
    ///            "addressTypeId": 1,
    ///            "address": "123 Work St",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///       ],
    ///       "username": "mathiasg",
    ///       "companyId": 1,
    ///       "company": "Company One",
    ///       "departmentId": 4,
    ///       "department": "Sales",
    ///       "created": "2024-07-13T15:40:05.9319504Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     },
    ///     {
    ///      ...
    ///     }
    ///     ]
    /// </remarks>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>), Description = "Return list of employees")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employees list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("getAll", Name = "GetAllEmployeesV3")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllAsync(ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAllAsync));
        var employees = await _repositoryFactory.EmployeeRepository.GetEmployeesAsync().ConfigureAwait(false);
        if (!employees.Any())
        {
            return NotFound(new { message = "The employees list is empty" });
        }
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>> (employees);
        return Ok(employeesDto);
    }

    /// <summary>
    /// Get a employee with Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v3/employees/6
    ///
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "1971-05-18T00:00:00",
    ///       "age": 50,
    ///       "addresses": [
    ///        {
    ///            "employeeId": 6,
    ///            "addressTypeId": 0,
    ///            "address": "000 Unknown St",
    ///            "created": "2024-07-13T15:40:05.9319142Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///            "employeeId": 6,
    ///            "addressTypeId": 1,
    ///            "address": "123 Work St",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///       ],
    ///       "username": "alanf",
    ///       "companyId": 1,
    ///       "company": "Company One",
    ///       "departmentId": 2,
    ///       "department": "Some Department",
    ///       "created": "2024-07-13T15:40:05.9319504Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="id" example="1">Employee Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeDto), Description = "Return employee")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("{id:int}", Name = "GetEmployeeByIdV3")]
    public async Task<ActionResult<EmployeeDto>> GetAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAsync));
        var employee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(id).ConfigureAwait(false);
        if (employee == null)
        {
            return NotFound(new { message = "The employee was not found" });
        }
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return Ok(employeeDto);
    }

    /// <summary>
    /// Update an employee 
    /// 
    /// All provided properties will be used to update the employee, so provide them if known. Properties not available in request body will remain unchanged.
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "employeeId": 1,
    ///       "firstName": "John",
    ///       "lastName": "Whyne",
    ///       "birthDate": "1965-05-31",
    ///       "addresses": 
    ///       [
    ///         {
    ///            "employeeId": 1,
    ///            "addressTypeId": 0,
    ///            "address": "Unknown address",
    ///         },
    ///         {
    ///            "employeeId": 1,
    ///            "addressTypeId": 1,
    ///            "address": "Work address"
    ///         },
    ///         {
    ///            "employeeId": 1,
    ///            "addressTypeId": 2,
    ///            "address": "Residential address"
    ///         }
    ///       ]
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 1,
    ///       "firstName": "John",
    ///       "lastName": "Whyne",
    ///       "birthDate": "1965-05-31T00:00:00",
    ///       "age": 55,
    ///       "addresses": [
    ///         {
    ///           "employeeId": 1,
    ///           "addressTypeId": 0,
    ///           "address": "Unknown address",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "employeeId": 1,
    ///           "addressTypeId": 1,
    ///           "address": "Work address",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "employeeId": 1,
    ///           "addressTypeId": 2,
    ///           "address": "Residential address",
    ///            "created": "2024-07-13T15:40:05.9319504Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///         }
    ///       ],
    ///       "username": "johnw",
    ///       "companyId": 1, 
    ///       "company": "Company One",
    ///       "departmentId": 1,
    ///       "department": "HR",    
    ///       "created": "2024-07-13T15:40:05.9319504Z",
    ///       "modified": "2024-07-13T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="employee"><see cref="EmployeeUpdateDto"/></param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeDto), Description = "Return updated employee")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPut("update", Name = "UpdateEmployeeV3")]
    public async Task<IActionResult> UpdateEmployeeAsync([FromBody] EmployeeUpdateDto employee, ApiVersion version)
    {
        Logger.LogDebug(nameof(UpdateEmployeeAsync));
        if (employee == null)
        {
            return BadRequest();
        }

        var repoEmployee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(employee.EmployeeId).ConfigureAwait(false);
        if (repoEmployee == null)
        {
            return NotFound(new { message = "The employee was not found" });
        }

        // Replace current addresses with new addresses provided - idempotent
        foreach (var address in repoEmployee.EmployeeAddresses)
        {
            _repositoryFactory.EmployeeAddressRepository.Remove(address); // remove existing addresses
        }
        repoEmployee.EmployeeAddresses = _mapper.Map<IList<EmployeeAddress>>(employee.Addresses);
        await _repositoryFactory.EmployeeAddressRepository
            .UpsertEmployeeAddressesAsync(repoEmployee.EmployeeAddresses); // using upsert for batch insert op

        // Handle updated employee
        repoEmployee.FirstName = employee.FirstName;
        repoEmployee.LastName = employee.LastName;
        repoEmployee.BirthDate = employee.BirthDate;
        await _repositoryFactory.EmployeeRepository
            .UpdateEmployeeAsync(repoEmployee);

        var updatedEmployee = await _repositoryFactory.EmployeeRepository
            .GetEmployeeAsync(repoEmployee.EmployeeId);
        var employeeDto = _mapper.Map<EmployeeDto>(updatedEmployee);
        return Ok(employeeDto);
    }

    /// <summary>
    /// 'Upsert' an employee. 
    /// 
    /// All provided properties will be used to upsert the employee, so provide them if known. Properties not available in request body will remain unchanged.
    /// The supplied address properties are inserted or updated. 
    /// If an address existed previously, but was not supplied in request body, it will continue to exist unchanged for employee. 
    /// Note: No addresses are removed using upsert.
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "employeeId": 1,
    ///       "firstName": "John",
    ///       "lastName": "Whyne",
    ///       "birthDate": "1965-05-31",
    ///       "addresses": 
    ///       [
    ///         {
    ///            "employeeId": 1,
    ///            "addressTypeId": 0,
    ///            "address": "Unknown address"
    ///         },
    ///         {
    ///            "employeeId": 1,
    ///            "addressTypeId": 2,
    ///            "address": "Residential address"
    ///         }
    ///       ]
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 1,
    ///       "firstName": "John",
    ///       "lastName": "Whyne",
    ///       "birthDate": "1965-05-31T00:00:00",
    ///       "age": 55,
    ///       "addresses": 
    ///       [
    ///         {
    ///           "employeeId": 1,
    ///           "addressTypeId": 0,
    ///           "address": "Unknown address",
    ///           "created": "2024-07-13T15:40:05.9319504Z",
    ///           "modified": "2024-07-13T15:40:05Z"
    ///         },
    ///         {
    ///           "employeeId": 1,
    ///           "addressTypeId": 1,
    ///           "address": "Work address"  (This address existed for employee and will remain after upsert if not supplied in request),
    ///           "created": "2024-07-13T15:40:05.9319504Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "employeeId": 1,
    ///           "addressTypeId": 2,
    ///           "address": "Residential address"
    ///           "created": "2024-07-13T15:40:05.9319504Z",
    ///           "modified": "2024-07-13T15:40:05Z"
    ///         }
    ///       ],
    ///       "username": "johnw",
    ///       "companyId": 1, 
    ///       "company": "Company One",
    ///       "departmentId": 1,
    ///       "department": "HR", 
    ///       "created": "2024-07-13T15:40:05.9319504Z",
    ///       "modified": "2024-07-13T15:40:05Z"
    ///     }
    /// </remarks>
    /// <param name="employee"><see cref="EmployeeUpdateDto"/></param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeDto), Description = "Return updated employee")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request was not valid")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPut("upsert", Name = "UpsertEmployeeV3")]
    public async Task<IActionResult> UpsertEmployeeAsync([FromBody] EmployeeUpdateDto employee, ApiVersion version)
    {
        Logger.LogDebug(nameof(UpsertEmployeeAsync));
        if (employee == null)
        {
            return BadRequest();
        }

        var repoEmployee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(employee.EmployeeId).ConfigureAwait(false);
        if (repoEmployee == null)
        {
            return NotFound(new { message = "The employee was not found" });
        }

        var changedEmployeeAddresses = _mapper.Map<IList<EmployeeAddress>>(employee.Addresses); 
        await _repositoryFactory.EmployeeAddressRepository
            .UpsertEmployeeAddressesAsync(changedEmployeeAddresses);
        repoEmployee.EmployeeAddresses.Clear();
        repoEmployee.EmployeeAddresses = await _repositoryFactory.EmployeeAddressRepository
            .GetEmployeeAddressesAsync(e => e.EmployeeId == employee.EmployeeId).ConfigureAwait(false);
        // Handle updated employee
        repoEmployee.FirstName = employee.FirstName;
        repoEmployee.LastName = employee.LastName;
        repoEmployee.BirthDate = employee.BirthDate;
        await _repositoryFactory.EmployeeRepository
            .UpdateEmployeeAsync(repoEmployee);

        var updatedEmployee = await _repositoryFactory.EmployeeRepository
            .GetEmployeeAsync(repoEmployee.EmployeeId);
        var employeeDto = _mapper.Map<EmployeeDto>(updatedEmployee);
        return Ok(employeeDto);
    }

    // TODO: Remove older SearchEmployeeAsync method after fully removing v2.1. Uses old Dto, and shouldn't.

    /// <summary>
    /// Search for employees
    /// </summary>
    /// <param name="searchCriteria">EmployeeSearchDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>), Description = "Returns a list of employees according to search criteria")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No employees were found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("search", Name = "SearchEmployeeV3")]
    public async Task<IActionResult> SearchAsync([FromQuery] EmployeeSearchDto searchCriteria, ApiVersion version)
    {
        Logger.LogDebug(nameof(SearchAsync));

        var employeeSearchCriteria = new EmployeeSearchCriteria
        {
            FirstName = searchCriteria.FirstName,
            LastName = searchCriteria.LastName,
            BirthDate = searchCriteria.BirthDate,
            Department = searchCriteria.Department,
            Username = searchCriteria.Username
        };
        var employees = await _repositoryFactory.EmployeeRepository.SearchEmployeesAsync(employeeSearchCriteria).ConfigureAwait(false);
        if (!employees.Any())
        {
            return NotFound(new { message = "No employees were found" });
        }
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return Ok(employeesDto);
    }
}
