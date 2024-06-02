using CompanyWebApi.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyWebApi.Persistence.Configurations
{
    public class AddressConfiguration
    {
        public AddressConfiguration(EntityTypeBuilder<Address> entity)
        {
            // Table
            entity.ToTable("Addresses");

            // Keys
            entity.HasKey(a => a.AddressId);

            // Properties
            entity.Property(a => a.FullAddress)
                .IsRequired()
                .HasMaxLength(255);

            // Relationships
            entity.HasMany(a => a.AddressRelations)
                .WithOne(ar => ar.Address)
                .HasForeignKey(ar => ar.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}