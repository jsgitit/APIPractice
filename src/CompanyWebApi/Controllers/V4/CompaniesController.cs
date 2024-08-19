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
public class CompaniesController : BaseController<CompaniesController>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;

    public CompaniesController(IRepositoryFactory repositoryFactory,
        IMapper mapper)
    {
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a new company
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///      "name": "Test Company"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///      "companyId": 12,
    ///      "name": "Test Company",
    ///      "created": "2024-07-13T14:25:00.6048657Z",
    ///      "modified": "1970-01-01T00:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="company">CompanyCreateDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CompanyDto), Description = "Returns a new company")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPost(Name = "CreateCompanyV4")]
    public async Task<ActionResult<CompanyDto>> CreateAsync([FromBody] CompanyCreateDto company, ApiVersion version)
    {
        Logger.LogDebug(nameof(CreateAsync));
        var newCompany = new Company
        {
            Name = company.Name
        };
        var repoCompany = await _repositoryFactory.CompanyRepository.AddCompanyAsync(newCompany).ConfigureAwait(false);
        var companyDto= _mapper.Map<CompanyDto>(repoCompany);
        return CreatedAtAction(nameof(GetCompanyByIdAsync), new { id = companyDto.CompanyId }, companyDto);
    }

    /// <summary>
    /// Get a company by Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/companies/1
    ///
    /// Sample response body:
    /// 
    ///     {
    ///      "companyId": 1,
    ///      "name": "Company 1",
    ///      "created": "2023-01-01T00:00:00Z",
    ///      "modified": "2024-07-23T21:26:40.4075049Z"
    ///     }
    /// </remarks>
    /// <param name="id" example="1">Company Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompanyDto), Description = "Return company")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("{id:int}", Name = "GetCompanyByIdV4")]
    public async Task<ActionResult<CompanyDto>> GetCompanyByIdAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetCompanyByIdAsync));
        var company = await _repositoryFactory.CompanyRepository.GetCompanyAsync(id).ConfigureAwait(false);
        if (company == null)
        {
            return NotFound(new { message = "The company was not found" });
        }
        var companyDto = _mapper.Map<CompanyDto>(company);
        return Ok(companyDto);
    }

    /// <summary>
    /// Gets all companies
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/companies
    ///
    /// Sample response body:
    /// 
    ///     {
    ///      "companies": [
    ///       {
    ///        "companyId": 1,
    ///        "name": "Company One",
    ///        "created": "2023-01-01T00:00:00Z",
    ///        "modified": "2024-08-06T14:37:16.6309998Z"
    ///       },
    ///       {
    ///        "companyId": 2,
    ///        "name": "Company Two",
    ///        "created": "2023-01-01T00:00:00Z",
    ///        "modified": "1970-01-01T00:00:00Z"
    ///       },
    ///       {
    ///        "companyId": 3,
    ///        "name": "Company Three",
    ///        "created": "2023-01-01T00:00:00Z",
    ///        "modified": "1970-01-01T00:00:00Z"
    ///       }
    ///      ]
    ///     }
    /// </remarks>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompanyListDto), Description = "Return list of companies")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The companies list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet(Name = "GetAllCompaniesV4")]
    public async Task<ActionResult<CompanyListDto>> GetAllCompaniesAsync(ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAllCompaniesAsync));
        var companies = await _repositoryFactory.CompanyRepository.GetCompaniesAsync().ConfigureAwait(false);
        if (!companies.Any())
        {
            return NotFound(new { message = "The companies list is empty" });
        }
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return Ok(new CompanyListDto { Companies = companiesDto });
    }

    /// <summary>
    ///     Gets a company with the specified Id, including full details.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///
    ///     GET /api/v4/companies/{id}/full
    ///
    /// Sample Response:
    ///
    ///     {
    ///      "companyId": 1,
    ///      "name": "Company One",
    ///      "employees": [
    ///       {
    ///        "employeeId": 1,
    ///        "firstName": "John",
    ///        "lastName": "Whyne",
    ///        "birthDate": "1991-08-07T00:00:00Z",
    ///        "age": 32,
    ///        "addresses": [
    ///         {
    ///          "employeeId": 1,
    ///          "addressTypeId": 1,
    ///          "address": "Kentucky, USA",
    ///          "created": "2023-01-01T00:00:00Z",
    ///          "modified": "1970-01-01T00:00:00Z"
    ///         }
    ///        ],
    ///        "username": "johnw",
    ///        "companyId": 1,
    ///        "company": "Company One",
    ///        "departmentId": 1,
    ///        "department": "Logistics",
    ///        "created": "2023-01-01T00:00:00Z",
    ///        "modified": "1970-01-01T00:00:00Z"
    ///       },
    ///       // ...
    ///      ],
    ///      "created": "2023-01-01T00:00:00Z",
    ///      "modified": "2024-08-06T14:37:16.6309998Z"
    ///     }
    /// </remarks>
    /// <param name="id" example="1">Company Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompanyFullDto), Description = "Return a company with full details")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("{id:int}/full", Name = "GetCompanyByIdFullV4")]
    public async Task<ActionResult<CompanyFullDto>> GetCompanyByIdFullAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(GetCompanyByIdFullAsync));
        var company = await _repositoryFactory.CompanyRepository.GetCompanyAsync(id).ConfigureAwait(false);
        if (company == null)
        {
            return NotFound(new { message = "The company was not found" });
        }
        var companyFullDto = _mapper.Map<CompanyFullDto>(company);
        return Ok(companyFullDto);
    }

    /// <summary>
    /// Gets all companies and their full details
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/companies/full
    ///
    /// Sample response body:
    /// 
    ///     {
    ///      "companies": [
    ///       {
    ///        "companyId": 1,
    ///        "name": "Company One",
    ///        "employees": [
    ///         {
    ///          "employeeId": 1,
    ///          "firstName": "John",
    ///          "lastName": "Whyne",
    ///          "birthDate": "1991-08-07T00:00:00Z",
    ///          "age": 32,
    ///          "addresses": [
    ///           {
    ///            "employeeId": 1,
    ///            "addressTypeId": 1,
    ///            "address": "Kentucky, USA",
    ///            "created": "2023-01-01T00:00:00Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///           }
    ///          ],
    ///          "username": "johnw",
    ///          "companyId": 1,
    ///          "company": "Company One",
    ///          "departmentId": 1,
    ///          "department": "Logistics",
    ///          "created": "2023-01-01T00:00:00Z",
    ///          "modified": "1970-01-01T00:00:00Z"
    ///         },
    ///         {
    ///          "employeeId": 4,
    ///          "firstName": "Alois",
    ///          "lastName": "Mock",
    ///          "birthDate": "1935-02-09T00:00:00Z",
    ///          "age": 89,
    ///          "addresses": [
    ///           {
    ///            "employeeId": 4,
    ///            "addressTypeId": 1,
    ///            "address": "Vienna, Austria",
    ///            "created": "2023-01-01T00:00:00Z",
    ///            "modified": "1970-01-01T00:00:00Z"
    ///           }
    ///          ],
    ///          "username": "aloism",
    ///          "companyId": 1,
    ///          "company": "Company One",
    ///          "departmentId": 2,
    ///          "department": "Administration",
    ///          "created": "2023-01-01T00:00:00Z",
    ///          "modified": "1970-01-01T00:00:00Z"
    ///         }
    ///        ],
    ///        "created": "2023-01-01T00:00:00Z",
    ///        "modified": "2024-08-06T14:37:16.6309998Z"
    ///       },
    ///       // ...
    ///      ]
    ///     }
    /// </remarks>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompanyFullListDto), Description = "Return list of companies with full details")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The companies list is empty")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpGet("full", Name = "GetAllCompaniesFullV4")]
    public async Task<ActionResult<CompanyFullListDto>> GetAllCompaniesFullAsync(ApiVersion version)
    {
        Logger.LogDebug(nameof(GetAllCompaniesFullAsync));
        var companies = await _repositoryFactory.CompanyRepository.GetCompaniesAsync().ConfigureAwait(false);
        if (!companies.Any())
        {
            return NotFound(new { message = "The companies list is empty" });
        }
        var companiesFullDto = _mapper.Map<IEnumerable<CompanyFullDto>>(companies);
        return Ok(new CompanyFullListDto { Companies = companiesFullDto });
    }

    /// <summary>
    /// Updates a company
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///      "companyId": 1,
    ///      "name": "Company One"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///      "companyId": 1,
    ///      "name": "Company One",
    ///      "created": "2023-01-01T00:00:00Z",
    ///      "modified": "2024-08-06T14:37:16.6309998Z"
    ///     }
    /// </remarks>
    /// <param name="company">CompanyUpdateDto model</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompanyDto), Description = "Return updated company")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The company was not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpPut(Name = "UpdateCompanyV4")]
    public async Task<IActionResult> UpdateAsync([FromBody] CompanyUpdateDto company, ApiVersion version)
    {
        Logger.LogDebug(nameof(UpdateAsync));
        var repoCompany = await _repositoryFactory.CompanyRepository.GetCompanyAsync(company.CompanyId).ConfigureAwait(false);
        if (repoCompany == null)
        {
            return NotFound(new { message = "The company was not found" });
        }
        repoCompany.Name = company.Name;
        await _repositoryFactory.CompanyRepository.UpdateAsync(repoCompany).ConfigureAwait(false);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        var companyDto = _mapper.Map<CompanyDto>(repoCompany);
        return Ok(companyDto);
    }

    /// <summary>
    /// Deletes a company with Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v4/companies/1
    ///
    /// Sample response body:
    ///     
    ///     204 No Content
    /// 
    /// </remarks>
    /// <param name="id" example="1">Company Id</param>
    /// <param name="version">API version</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Company was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No company was found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized user")]
    [HttpDelete("{id:int}", Name = "DeleteCompanyByIdV4")]
    public async Task<IActionResult> DeleteAsync(int id, ApiVersion version)
    {
        Logger.LogDebug(nameof(DeleteAsync));
        var company = await _repositoryFactory.CompanyRepository.GetCompanyAsync(id).ConfigureAwait(false);
        if (company == null)
        {
            return NotFound(new { message = "The company was not found" });
        }
        _repositoryFactory.CompanyRepository.Remove(company);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return NoContent();
    }
}