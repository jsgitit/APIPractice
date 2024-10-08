﻿using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CompanyWebApi.Contracts.Dto.V3
{
	/// <summary>
	/// Employee Update Data Transfer Object
	/// </summary>
    [Serializable]
    public class EmployeeUpdateDto
	{
		[Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
		public int EmployeeId { get; set; }

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
        public IList<EmployeeAddressUpdateDto> Addresses { get; set; }
    }
}