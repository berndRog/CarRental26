using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CarRentalApi.Data.Database;

public sealed class ConfigCustomers: IEntityTypeConfiguration<Customer> {
   
   public void Configure(EntityTypeBuilder<Customer> b) {
      // Table
      b.ToTable("Customers");
     
      // Primary Key
      b.HasKey(x => x.Id);
      
      // Properties
      b.Property(x => x.Id).ValueGeneratedNever();
      
      // Customer: Reservations = 1 : 1..*
      b.HasMany(c => c.Reservations)
         .WithOne(r => r.Customer)
         .HasForeignKey(r => r.CustomerId)
         .OnDelete(DeleteBehavior.Restrict)
         .IsRequired();

      // Customers : Rentals = 1 : 1..*
      b.HasMany(c => c.Rentals)
         .WithOne(r => r.Customer)
         .HasForeignKey(r => r.CustomerId)
         .OnDelete(DeleteBehavior.Restrict)
         .IsRequired();
   }
}