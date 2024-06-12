using CompanyWebApi.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyWebApi.Persistence.Configurations
{
    public class EmployeeAddressConfiguration
    {
        public EmployeeAddressConfiguration(EntityTypeBuilder<EmployeeAddress> entity)
        {
            // Table
            entity.ToTable("EmployeeAddresses");

            // Keys
            entity.HasKey(empa => new { empa.EmployeeId, empa.AddressTypeId});

            // Properties
            entity.Property(empa => empa.Address)
                .IsRequired();

            // Relationships
            entity.HasOne(empa => empa.Employee)
                .WithMany(emp => emp.EmployeeAddresses)
                .HasForeignKey(empa => empa.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
