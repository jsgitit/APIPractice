using Asp.Versioning;
using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
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

namespace CompanyWebApi.Controllers.V4;

[ApiAuthorization]
[ApiController]
[ApiVersion("4.0")]
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
    ///       "firstName": "Dan",
    ///       "lastName": "Thompson",
    ///       "birthDate": "2020-08-19T20:45:40.278Z",
    ///       "companyId": 1,
    ///       "departmentId": 1,
    ///       "addresses": [
    ///         {
    ///           "addressTypeId": 0,
    ///           "address": "Address Unknown"
    ///         }
    ///       ],
    ///       "username": "dthompson",
    ///       "password": "dttemppwd"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 10,
    ///       "firstName": "Dan",
    ///       "lastName": "Thompson",
    ///       "birthDate": "2020-08-19T20:45:40.278Z",
    ///       "age": 4,
    ///       "addresses": [
    ///         {
    ///           "employeeId": 10,
    ///           "addressTypeId": 0,
    ///           "address": "Address Unknown",
    ///           "created": "2024-08-19T20:48:00.1358688Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         }
    ///       ],
    ///       "username": "dthompson",
    ///       "companyId": 1,
    ///       "company": "Company One",
    ///       "departmentId": 1,
    ///       "department": "Logistics",
    ///       "created": "2024-08-19T20:48:00.153139Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    ///
    /// </remarks>
    /// <param name="employee">EmployeeCreateDto model</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EmployeeFullDto), Description = "Returns a new employee")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Company or Department was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPost(Name = "CreateEmployee")]
    public async Task<IActionResult> CreateAsync([FromBody] EmployeeCreateDto employee)
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
        var employeeFullDto = _mapper.Map<EmployeeFullDto>(repoEmployee);
        return CreatedAtRoute(nameof(GetEmployeeByIdFullAsync), new { id = employeeFullDto.EmployeeId}, employeeFullDto);
    }

    /// <summary>
    /// Get an employee by Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/v4/employees/6
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "1984-06-15T00:00:00Z",
    ///       "companyId": 2,
    ///       "company": "Company Two",
    ///       "departmentId": 6,
    ///       "department": "Customer Support",
    ///       "created": "2023-01-01T00:00:00Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="id" example="1">Employee Id</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeDto), Description = "Return employee")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("{id:int}", Name = "GetEmployeeByIdAsync")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        Logger.LogDebug(nameof(GetEmployeeByIdAsync));
        var employee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(id).ConfigureAwait(false);
        if (employee == null)
        {
            return NotFound(new { message = "The employee was not found" });
        }
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return Ok(employeeDto);
    }

    /// <summary>
    /// Gets all employees
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/v4/employees
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employees": [
    ///         {
    ///           "employeeId": 5,
    ///           "firstName": "Gertraud",
    ///           "lastName": "Bochold",
    ///           "birthDate": "2001-03-04T00:00:00Z",
    ///           "companyId": 2,
    ///           "company": "Company Two",
    ///           "departmentId": 6,
    ///           "department": "Customer Support",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "employeeId": 6,
    ///           "firstName": "Alan",
    ///           "lastName": "Ford",
    ///           "birthDate": "1984-06-15T00:00:00Z",
    ///           "companyId": 2,
    ///           "company": "Company Two",
    ///           "departmentId": 6,
    ///           "department": "Customer Support",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         //...
    ///       ]
    ///     }
    /// </remarks>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeListDto), Description = "Return list of employees")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employees list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet(Name = "GetAllEmployeesAsync")]
    public async Task<ActionResult<EmployeeListDto>> GetAllEmployeesAsync()
    {
        Logger.LogDebug(nameof(GetAllEmployeesAsync));
        var employees = await _repositoryFactory.EmployeeRepository.GetEmployeesAsync().ConfigureAwait(false);
        if (!employees.Any())
        {
            return NotFound(new { message = "The employees list is empty" });
        }
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return Ok(new EmployeeListDto { Employees = employeesDto });
    }

    /// <summary>
    /// Get an employee with specified ID, including full details
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/v4/employees/6/full
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "1984-06-15T00:00:00Z",
    ///       "age": 40,
    ///       "addresses": [
    ///         {
    ///           "employeeId": 6,
    ///           "addressTypeId": 1,
    ///           "address": "Milano, Italy",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         }
    ///       ],
    ///       "username": "alanf",
    ///       "companyId": 2,
    ///       "company": "Company Two",
    ///       "departmentId": 6,
    ///       "department": "Customer Support",
    ///       "created": "2023-01-01T00:00:00Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="id" example="1">Employee Id</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeFullDto), Description = "Return employee with full details")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("{id:int}/full", Name = "GetEmployeeByIdFullAsync")]
    public async Task<ActionResult<EmployeeFullDto>> GetEmployeeByIdFullAsync(int id)
    {
        Logger.LogDebug(nameof(GetEmployeeByIdFullAsync));
        var employee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(id).ConfigureAwait(false);
        if (employee == null)
        {
            return NotFound(new { message = "The employee was not found" });
        }
        var employeeFullDto = _mapper.Map<EmployeeFullDto>(employee);
        return Ok(employeeFullDto);
    }

    /// <summary>
    /// Gets all employees and their full details
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/v4/employees/full
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employees": [
    ///         {
    ///           "employeeId": 5,
    ///           "firstName": "Gertraud",
    ///           "lastName": "Bochold",
    ///           "birthDate": "2001-03-04T00:00:00Z",
    ///           "age": 23,
    ///           "addresses": [
    ///             {
    ///               "employeeId": 5,
    ///               "addressTypeId": 1,
    ///               "address": "Cologne, Germany",
    ///               "created": "2023-01-01T00:00:00Z",
    ///               "modified": "1970-01-01T00:00:00Z"
    ///             }
    ///           ],
    ///           "username": "gertraudb",
    ///           "companyId": 2,
    ///           "company": "Company Two",
    ///           "departmentId": 6,
    ///           "department": "Customer Support",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "employeeId": 6,
    ///           "firstName": "Alan",
    ///           "lastName": "Ford",
    ///           "birthDate": "1984-06-15T00:00:00Z",
    ///           "age": 40,
    ///           "addresses": [
    ///             {
    ///               "employeeId": 6,
    ///               "addressTypeId": 1,
    ///               "address": "Milano, Italy",
    ///               "created": "2023-01-01T00:00:00Z",
    ///               "modified": "1970-01-01T00:00:00Z"
    ///             }
    ///           ],
    ///           "username": "alanf",
    ///           "companyId": 2,
    ///           "company": "Company Two",
    ///           "departmentId": 6,
    ///           "department": "Customer Support",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         //...
    ///       ]
    ///     }
    /// </remarks>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeFullListDto), Description = "Return list of employees with full details")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employees list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("full", Name = nameof(GetAllEmployeesFullAsync))]
    public async Task<ActionResult<EmployeeFullListDto>> GetAllEmployeesFullAsync()
    {
        Logger.LogDebug(nameof(GetAllEmployeesFullAsync));
        var employees = await _repositoryFactory.EmployeeRepository.GetEmployeesAsync().ConfigureAwait(false);
        if (!employees.Any())
        {
            return NotFound(new { message = "The employees list is empty" });
        }
        var employeesFullDto = _mapper.Map<IEnumerable<EmployeeFullDto>>(employees);
        return Ok(new EmployeeFullListDto { Employees = employeesFullDto });
    }

    /// <summary>
    /// Update an employee 
    /// 
    /// All provided properties will be used to update the employee, so provide them if known. Properties not available in request body will remain unchanged.
    /// </summary>
    /// <remarks>
    /// 
    /// Sample request body:
    /// 
    ///     PUT /api/v4/employee/6
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "2000-08-19T21:43:42.364Z",
    ///       "addresses": [
    ///         {
    ///           "employeeId": 6,
    ///           "addressTypeId": 0,
    ///           "address": "123 Unknown Street"
    ///         },
    ///         {
    ///           "employeeId": 6,
    ///           "addressTypeId": 1,
    ///           "address": "123 Work Street"
    ///         }
    ///       ]
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 6,
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "birthDate": "2000-08-19T21:43:42.364Z",
    ///       "age": 23,
    ///       "addresses": [
    ///         {
    ///           "employeeId": 6,
    ///           "addressTypeId": 0,
    ///           "address": "123 Unknown Street",
    ///           "created": "2024-08-19T21:43:42.364Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "employeeId": 6,
    ///           "addressTypeId": 1,
    ///           "address": "123 Work Street",
    ///           "created": "2024-08-19T21:43:42.364Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         }
    ///       ],
    ///       "username": "alanf",
    ///       "companyId": 2,
    ///       "company": "Company Two",
    ///       "departmentId": 6,
    ///       "department": "Customer Support",
    ///       "created": "2023-01-01T00:00:00Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="id" example="6">Employee Id</param>
    /// <param name="employee">EmployeeUpdateDto model</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeFullDto), Description = "Return updated employee")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPut(Name = nameof(UpdateEmployeeAsync))]
    public async Task<IActionResult> UpdateEmployeeAsync([FromBody] EmployeeUpdateDto employee)
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
        var employeeFullDto = _mapper.Map<EmployeeFullDto>(updatedEmployee);
        return Ok(employeeFullDto);
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
    ///     PUT /api/v4/employee/6/upsert
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
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmployeeFullDto), Description = "Return updated employee")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request was not valid")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPut("upsert", Name = nameof(UpsertEmployeeAsync))]
    public async Task<IActionResult> UpsertEmployeeAsync([FromBody] EmployeeUpdateDto employee)
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
        var employeeFullDto = _mapper.Map<EmployeeFullDto>(updatedEmployee);
        return Ok(employeeFullDto);
    }

    /// <summary>
    /// Search for employees
    /// </summary>
    /// <param name="searchCriteria">EmployeeSearchDto model</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeFullDto>), Description = "Returns a list of employees according to search criteria")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No employees were found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("search", Name = nameof(SearchEmployeeAsync))]
    public async Task<IActionResult> SearchEmployeeAsync([FromQuery] EmployeeSearchDto searchCriteria)
    {
        // TODO: Remove older SearchEmployeeAsync method after fully removing v2.1. Uses old Dto, and shouldn't.
        Logger.LogDebug(nameof(SearchEmployeeAsync));

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
        var employeesFullDto = _mapper.Map<IEnumerable<EmployeeFullDto>>(employees);
        return Ok(employeesFullDto);
    }

    /// <summary>
    /// Deletes a employee with Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v4/employees/6
    ///
    /// Sample response body:
    ///     
    ///    Code 204 No Content
    /// 
    /// </remarks>
    /// <param name="id" example="6">Employee Id</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Employee was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No employee was found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpDelete("{id:int}", Name = nameof(DeleteEmployeeByIdAsync))]
    public async Task<IActionResult> DeleteEmployeeByIdAsync(int id)
    {
        Logger.LogDebug(nameof(DeleteEmployeeByIdAsync));
        var employee = await _repositoryFactory.EmployeeRepository.GetEmployeeAsync(id).ConfigureAwait(false);
        if (employee == null)
        {
            return NotFound(new { message = "No employee was found" });
        }
        _repositoryFactory.EmployeeRepository.Remove(employee);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return NoContent();
    }
}
