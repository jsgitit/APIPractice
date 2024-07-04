using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace CompanyWebApi.Contracts.Dto.V3;

/// <summary>
/// Employee Data Transfer Object
/// </summary>
[Serializable]
public class EmployeeDto
{
	[Key]
	[Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
	public int EmployeeId { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public DateTime BirthDate { get; set; }

	public int Age { get; set; }

	[Required]
	[StringLength(255, MinimumLength = 1)]
	public IList<EmployeeAddressDto> Addresses { get; set; }

	public string Username { get; set; }

	public int CompanyId { get; set; }

	public string Company { get; set; }

	public int DepartmentId { get; set; }

	public string Department { get; set; }
}