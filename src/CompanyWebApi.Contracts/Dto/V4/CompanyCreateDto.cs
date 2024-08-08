using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApi.Contracts.Dto.V4
{
    /// <summary>
    /// Company Creation Data Transfer Object
    /// </summary>
    [Serializable]
    public class CompanyCreateDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Company name length must be in range 5-50 chars")]
        public string Name { get; set; }
    }
}