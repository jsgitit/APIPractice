using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApi.Contracts.Dto.V4;

/// <summary>
/// Employee Create Data Transfer Object
/// </summary>
[Serializable]
public class EmployeeCreateDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; }

    [Required]
    [Range(typeof(DateTime), "01/01/1900", "01/01/2100")]
    public DateTime BirthDate { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public IList<EmployeeAddressCreateWithoutEmployeeIdDto> Addresses { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
}