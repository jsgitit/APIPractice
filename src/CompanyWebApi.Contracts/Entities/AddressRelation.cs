using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyWebApi.Contracts.Entities;
[Serializable]
public class AddressRelation
{
    [Key, Column(Order = 0)]
    [Required]
    public Guid AddressId { get; set; }

    [Key, Column(Order = 1)]
    [Required]
    public EntityIdType EntityIdType { get; set; }

    [Key, Column(Order = 2)]
    [Required]
    public int EntityId { get; set; }

    [ForeignKey("AddressId")]
    public Address Address { get; set; }
}
