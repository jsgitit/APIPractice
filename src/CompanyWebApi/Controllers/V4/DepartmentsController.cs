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
public class DepartmentsController : BaseController<DepartmentsController>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;

    public DepartmentsController(IRepositoryFactory repositoryFactory,
        IMapper mapper)
    {
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a new department
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///        "companyId" : 1, 
    ///        "name": "Test Department"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///         "departmentId": 10,
    ///         "name": "Test Department",
    ///         "companyId": 1, 
    ///         "companyName": "Test Company",
    ///         "created": "2024-08-10T15:00:45.4881589Z",
    ///         "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// 
    /// </remarks>
    /// <param name="department">DepartmentDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(DepartmentDto), Description = "Returns a new department")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Company was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPost(Name = "CreateDepartmentV4")]
    public async Task<IActionResult> CreateDepartmentAsync([FromBody] DepartmentCreateDto department, ApiVersion version)
    {
        Logger.LogDebug(nameof(CreateDepartmentAsync));
        if (!await _repositoryFactory.CompanyRepository.ExistsAsync(c => c.CompanyId == department.CompanyId).ConfigureAwait(false))
        {
            return NotFound(new { message = $"The Company with id {department.CompanyId} was not found" });
        }

        var newDepartment = new Department()
        {
            CompanyId = department.CompanyId,
            Name = department.Name
        };

        var repoDepartment = await _repositoryFactory.DepartmentRepository.AddDepartmentAsync(newDepartment).ConfigureAwait(false);
        var departmentDto = _mapper.Map<DepartmentDto>(repoDepartment);
        return CreatedAtAction(nameof(GetDepartmentByIdAsync), new { id = departmentDto.DepartmentId }, departmentDto);
    }

    /// <summary>
    /// Get a department with id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/departments/1
    ///
    /// Sample response body:
    /// 
    ///     {
    ///       "departmentId": 1,
    ///       "name": "Logistics",
    ///       "companyId": 1,
    ///       "companyName": "Company One",
    ///       "created": "2023-01-01T00:00:00Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// 
    /// </remarks>
    /// <param name="id" example="1">Department Id</param>
    /// <param name="version">API version</param>
    [HttpGet("{id:int}", Name = "GetDepartmentByIdV4")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DepartmentDto), Description = "Return department")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The department was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentByIdAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetDepartmentByIdAsync));
        var department = await _repositoryFactory.DepartmentRepository.GetDepartmentAsync(id).ConfigureAwait(false);
        if (department == null)
        {
            return NotFound(new { message = "The department was not found" });
        }
        var departmentDto = _mapper.Map<DepartmentDto>(department);
        return Ok(departmentDto);
    }

    /// <summary>
    /// Gets all departments
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/departments/
    ///
    /// Sample response body:
    ///
    ///     {
    ///       "departments": [
    ///         {
    ///           "departmentId": 1,
    ///           "name": "Logistics",
    ///           "companyId": 1,
    ///           "companyName": "Company One",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "departmentId": 2,
    ///           "name": "Administration",
    ///           "companyId": 1,
    ///           "companyName": "Company One",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         // ...
    ///       ]
    ///     }
    ///
    /// </remarks>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DepartmentListDto), Description = "Return list of departments")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The departments list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet(Name = "GetAllDepartmentsV4")]
    public async Task<ActionResult<DepartmentListDto>> GetAllDepartmentsAsync(ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAllDepartmentsAsync));
        var departments = await _repositoryFactory.DepartmentRepository.GetDepartmentsAsync().ConfigureAwait(false);
        if (!departments.Any())
        {
            return NotFound(new { message = "The departments list is empty" });
        }
        var departmentsDto = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return Ok(new DepartmentListDto { Departments = departmentsDto });
    }

    /// <summary>
    /// Gets a department with the specified Id, including full details.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///
    ///     GET /api/v4/departments/{id}/full
    ///
    /// Sample Response:
    ///
    ///     {
    ///       "departmentId": 1,
    ///       "name": "Logistics",
    ///       "companyId": 1,
    ///       "companyName": "Company One",
    ///       "employees": [
    ///         {
    ///           "employeeId": 1,
    ///           "firstName": "John",
    ///           "lastName": "Whyne",
    ///           "birthDate": "1991-08-07T00:00:00Z",
    ///           "age": 33,
    ///           "addresses": [
    ///             {
    ///               "employeeId": 1,
    ///               "addressTypeId": 1,
    ///               "address": "Kentucky, USA",
    ///               "created": "2023-01-01T00:00:00Z",
    ///               "modified": "1970-01-01T00:00:00Z"
    ///             }
    ///           ],
    ///           "username": "johnw",
    ///           "companyId": 1,
    ///           "company": "Company One",
    ///           "departmentId": 1,
    ///           "department": "Logistics",
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         // ...
    ///       ],
    ///       "created": "2023-01-01T00:00:00Z",
    ///       "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// 
    /// </remarks>
    /// <param name="id" example="1">Department Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DepartmentFullDto), Description = "Return a department with full details")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The department was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("{id:int}/full", Name = "GetDepartmentByIdFullV4")]
    public async Task<ActionResult<DepartmentFullDto>> GetDepartmentByIdFullAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetDepartmentByIdFullAsync));
        var department = await _repositoryFactory.DepartmentRepository.GetDepartmentAsync(id).ConfigureAwait(false);
        if (department == null)
        {
            return NotFound(new { message = "The department was not found" });
        }
        var departmentFullDto = _mapper.Map<DepartmentFullDto>(department);
        return Ok(departmentFullDto);
    }

    /// <summary>
    /// Gets all Departments and their full details
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/departments/full
    ///
    /// Sample response body:
    /// 
    ///     {
    ///       "departments": [
    ///         {
    ///           "departmentId": 1,
    ///           "name": "Logistics",
    ///           "companyId": 1,
    ///           "companyName": "Company One",
    ///           "employees": [
    ///             {
    ///               "employeeId": 1,
    ///               "firstName": "John",
    ///               "lastName": "Whyne",
    ///               "birthDate": "1991-08-07T00:00:00Z",
    ///               "age": 33,
    ///               "addresses": [
    ///                 {
    ///                   "employeeId": 1,
    ///                   "addressTypeId": 1,
    ///                   "address": "Kentucky, USA",
    ///                   "created": "2023-01-01T00:00:00Z",
    ///                   "modified": "1970-01-01T00:00:00Z"
    ///                 }
    ///               ],
    ///               "username": "johnw",
    ///               "companyId": 1,
    ///               "company": "Company One",
    ///               "departmentId": 1,
    ///               "department": "Logistics",
    ///               "created": "2023-01-01T00:00:00Z",
    ///               "modified": "1970-01-01T00:00:00Z"
    ///             }
    ///           ],
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///           "departmentId": 2,
    ///           "name": "Administration",
    ///           "companyId": 1,
    ///           "companyName": "Company One",
    ///           "employees": [
    ///             {
    ///               "employeeId": 4,
    ///               "firstName": "Alois",
    ///               "lastName": "Mock",
    ///               "birthDate": "1935-02-09T00:00:00Z",
    ///               "age": 89,
    ///               "addresses": [
    ///                 {
    ///                   "employeeId": 4,
    ///                   "addressTypeId": 1,
    ///                   "address": "Vienna, Austria",
    ///                   "created": "2023-01-01T00:00:00Z",
    ///                   "modified": "1970-01-01T00:00:00Z"
    ///                 }
    ///               ],
    ///               "username": "aloism",
    ///               "companyId": 1,
    ///               "company": "Company One",
    ///               "departmentId": 2,
    ///               "department": "Administration",
    ///               "created": "2023-01-01T00:00:00Z",
    ///               "modified": "1970-01-01T00:00:00Z"
    ///             }
    ///           ],
    ///           "created": "2023-01-01T00:00:00Z",
    ///           "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         // ...
    ///       ]
    ///     }
    ///
    /// </remarks>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DepartmentFullListDto), Description = "Return list of departments with full details")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The departments list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("full", Name = "GetAllDepartmentsFullV4")]
    public async Task<ActionResult<DepartmentFullListDto>> GetAllDepartmentsFullAsync(ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAllDepartmentsFullAsync));
        var departments = await _repositoryFactory.DepartmentRepository.GetDepartmentsAsync().ConfigureAwait(false);
        if (!departments.Any())
        {
            return NotFound(new { message = "The departments list is empty" });
        }
        var departmentsDto = _mapper.Map<IEnumerable<DepartmentFullDto>>(departments);
        return Ok(new DepartmentFullListDto { Departments = departmentsDto });
    }

    /// <summary>
    /// Updates a department
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "departmentId": 1,
    ///       "name": "NEW DEPARTMENT"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "departmentId": 1,
    ///       "name": "NEW DEPARTMENT",
    ///       "companyId": 1,
    ///       "companyName": "Company One",
    ///       "created": "2024-06-18T17:53:51.9976026",
    ///       "modified": "2024-06-18T17:53:51.9976028"
    ///     }
    /// 
    /// </remarks>
    /// <param name="department">DepartmentUpdateDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DepartmentDto), Description = "Return updated department")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The department was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPut(Name = "UpdateDepartmentV4")]
    public async Task<IActionResult> UpdateAsync([FromBody] DepartmentUpdateDto department, ApiVersion version)
    {
        Logger.LogDebug(nameof(UpdateAsync));
        var repoDepartment = await _repositoryFactory.DepartmentRepository.GetDepartmentAsync(department.DepartmentId).ConfigureAwait(false);
        if (repoDepartment == null)
        {
            return NotFound(new { message = "The department was not found" });
        }

        // Update Department's name
        repoDepartment.Name = department.Name;

        await _repositoryFactory.DepartmentRepository.UpdateAsync(repoDepartment).ConfigureAwait(false);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);

        var departmentDto = _mapper.Map<DepartmentDto>(repoDepartment);
        return Ok(departmentDto);
    }

    /// <summary>
    /// Deletes a department with id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v4/departments/1
    ///
    /// Sample response body:
    ///     
    ///     204 No Content
    /// 
    /// </remarks>
    /// <param name="id" example="1">Department Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Department was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No department was found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpDelete("{id:int}", Name = "DeleteDepartmentByIdV4")]
    public async Task<IActionResult> DeleteAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(DeleteAsync));
        var department = await _repositoryFactory.DepartmentRepository.GetDepartmentAsync(id).ConfigureAwait(false);
        if (department == null)
        {
            return NotFound(new { message = "The department was not found" });
        }
        _repositoryFactory.DepartmentRepository.Remove(department);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return NoContent();
    }
}