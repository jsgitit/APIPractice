using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWebApi.Contracts.Dto;
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
}
