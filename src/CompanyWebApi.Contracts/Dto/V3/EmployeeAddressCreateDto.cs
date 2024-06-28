using System.ComponentModel.DataAnnotations;

namespace CompanyWebApi.Contracts.Dto.V3;
public class EmployeeAddressCreateDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public AddressType AddressTypeId { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Address { get; set; }
}
