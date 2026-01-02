using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CarRentalApi.Data.Database;

public class ConfigAdmins : IEntityTypeConfiguration<Admin> {
   public void Configure(EntityTypeBuilder<Admin> b) {
      
      // Tablename
      b.ToTable("Admins");
      
      // Primary Key is inherited from Person

      // Properties
      b.Property(a => a.AdminRights)
         .IsRequired();
   }
}
