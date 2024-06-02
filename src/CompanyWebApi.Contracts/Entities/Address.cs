using CompanyWebApi.Contracts.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApi.Contracts.Entities;
[Serializable]
public class Address : BaseAuditEntity
{
    [Key]
    public Guid AddressId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullAddress { get; set; }

    // Navigation property for AddressRelations
    public ICollection<AddressRelation> AddressRelations { get; set; }

}