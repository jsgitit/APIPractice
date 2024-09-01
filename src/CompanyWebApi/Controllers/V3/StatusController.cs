using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Controllers.Base;
using CompanyWebApi.Services.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using Asp.Versioning;

namespace CompanyWebApi.Controllers.V3
{
	[ApiController]
    [ApiVersion("3.0", Deprecated = true)]
    [Produces("application/json")]
	[EnableCors("EnableCORS")]
	[ServiceFilter(typeof(ValidModelStateAsyncActionFilter))]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class StatusController : BaseController<StatusController>
	{
		/// <summary>
		/// Gets API status
		/// </summary>
		/// <param name="version">API version</param>
		/// <returns>Status info</returns>
		[HttpGet]
		public ActionResult<StatusResponse> Get(ApiVersion version)
		{
			var assemblyName = typeof(Startup).Assembly.GetName().Name;
			var assemblyVersion = typeof(Startup).Assembly.GetName().Version;
			var result = new StatusResponse
			{
				AssemblyName = assemblyName,
				AssemblyVersion = $"{assemblyVersion?.Major}.{assemblyVersion?.Minor}.{assemblyVersion?.Build}",
				StartTime = Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
				Host = Environment.MachineName
			};
			return Ok(result);
		}
	}
}
