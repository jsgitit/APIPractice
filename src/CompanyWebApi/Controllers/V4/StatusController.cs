using Asp.Versioning;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Controllers.Base;
using CompanyWebApi.Services.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace CompanyWebApi.Controllers.V4;

[ApiController]
[ApiVersion("4.0")]
[Produces("application/json")]
[EnableCors("EnableCORS")]
[ServiceFilter(typeof(ValidModelStateAsyncActionFilter))]
[Route("api/v{version:apiVersion}/[controller]")]
public class StatusController : BaseController<StatusController>
{
    /// <summary>
    /// Gets API status
    /// </summary>
    /// <returns>Status info</returns>
    [HttpGet]
    public ActionResult<StatusResponse> GetStatus()
    {
        var assemblyName = typeof(Startup).Assembly.GetName().Name;
        var assemblyVersion = typeof(Startup).Assembly.GetName().Version;
        var result = new StatusResponseModel
        {
            AssemblyName = assemblyName,
            AssemblyVersion = $"{assemblyVersion?.Major}.{assemblyVersion?.Minor}.{assemblyVersion?.Build}",
            StartTime = Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
            Host = Environment.MachineName
        };
        return Ok(result);
    }
}