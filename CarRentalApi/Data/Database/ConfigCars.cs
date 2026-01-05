using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CarRentalApi.Data.Database;

public sealed class ConfigCars: IEntityTypeConfiguration<Car> {
   
   public void Configure(EntityTypeBuilder<Car> b) {
      // Table
      b.ToTable("Cars");
      // Primary Key
      b.HasKey(x => x.Id);
      
      // Properties
      b.Property(x => x.Id)
         .ValueGeneratedNever();
      b.Property(x => x.Manufacturer)
         .IsRequired()
         .HasMaxLength(100);
      b.Property(x => x.Model)
         .IsRequired()
         .HasMaxLength(100);
      b.Property(x => x.LicensePlate)
         .IsRequired()
         .HasMaxLength(32);
      b.HasIndex(x => x.LicensePlate)
         .IsUnique();
      b.Property(x => x.CarCategory)
         .IsRequired();
      b.Property(x => x.CarStatus)
         .IsRequired();
      
      // Index "available cars by category"
      b.HasIndex(x => new { Category = x.CarCategory, Status = x.CarStatus });
      
      // Relationship Cars : Rentals = 1 : 1..*
      // --------------------------------------------------------------------------------
      
#if OOP_MODE
      // Relationships with object graphs
      // Cars : Rentals = 1 : 1..*
      b.HasMany(c => c.Rentals)
         .WithOne(r => r.Car)
         .HasForeignKey(r => r.CarId)
         .OnDelete(DeleteBehavior.Restrict)
         .IsRequired();
#elif DDD_MODE
      b.HasMany<Rental>()
         .WithOne(r => r.Car)
         .HasForeignKey(r => r.CarId);
         .OnDelete(DeleteBehavior.Restrict);
#else
      #error "Define either OOP_MODE or DDD_MODE in .csproj"
#endif
   }
}