using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using CompanyWebApi.Contracts.Entities.Base;

namespace CompanyWebApi.Contracts.Entities
{
	[Serializable]
	public class EmployeeAddress : BaseAuditEntity
	{
		[Key, ForeignKey(nameof(Employee))]
		public int EmployeeId { get; set; }

		[Key]
		public AddressType AddressTypeId { get; set; } = AddressType.Work;

		// Inverse navigation property
		public Employee Employee { get; set; }

		[Required]
        [StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]

        public string Address {  get; set; }
	}
}
