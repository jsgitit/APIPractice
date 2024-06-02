using CompanyWebApi.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyWebApi.Persistence.Configurations
{
    public class AddressRelationConfiguration
    {
        public AddressRelationConfiguration(EntityTypeBuilder<AddressRelation> entity)
        {
            // Table
            entity.ToTable("AddressRelations");

            // Keys
            entity.HasKey(ar => new { ar.EntityIdType, ar.EntityId, ar.AddressId });

            // Properties
            entity.Property(ar => ar.EntityIdType)
                .IsRequired();

            entity.Property(ar => ar.EntityId)
                .IsRequired();

            entity.Property(ar => ar.AddressId)
                .IsRequired();

            // Relationships
            entity.HasOne(ar => ar.Address)
                .WithMany(a => a.AddressRelations)
                .HasForeignKey(ar => ar.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            // Assuming EntityIdType is an enum stored as int
            entity.Property(ar => ar.EntityIdType).HasConversion<int>();
        }
    }
}