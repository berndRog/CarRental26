using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CarRentalApi.Data.Database;

public sealed class ConfigPeople : IEntityTypeConfiguration<Person> {
   public void Configure(EntityTypeBuilder<Person> b) {
      // Table für Basistyp
      b.ToTable("People");

      // Primary Key auf Basistyp
      b.HasKey(x => x.Id);

      // Properties
      b.Property(x => x.Id).ValueGeneratedNever();

      // Map common properties on Person (adjust names to your domain model)
      b.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
      b.Property(p => p.LastName).IsRequired().HasMaxLength(100);
      b.Property(p => p.Email).IsRequired().HasMaxLength(200);

      // Address is a value object and owned type, so it applies to all derived types
      b.OwnsOne(p => p.Address, a => {
         a.Property(x => x.Street).HasMaxLength(200);
         a.Property(x => x.PostalCode).HasMaxLength(20);
         a.Property(x => x.City).HasMaxLength(100);
      });
   }
}