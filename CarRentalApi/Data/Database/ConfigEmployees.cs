using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CarRentalApi.Data.Database;

public class ConfigEmployees : IEntityTypeConfiguration<Employee> {
   public void Configure(EntityTypeBuilder<Employee> b) {
      // Tablename
      b.ToTable("Employees");
      
      // Primary Key is inherited from Person

      // Properties
      b.Property(e => e.PersonnelNumber)
         .IsRequired()
         .HasMaxLength(16);
   }
}
