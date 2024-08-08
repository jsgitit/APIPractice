using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApi.Contracts.Dto.V4;
public class EmployeeAddressDto
{
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int EmployeeId { get; set; }

    [Required]
    public AddressType AddressTypeId { get; set; }
    
    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Address { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
