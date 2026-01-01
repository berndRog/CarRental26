using CarRentalApi.Data.Database.Configurations;
using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CarRentalApi.Data.Database;

public sealed class CarRentalDbContext(
   DbContextOptions<CarRentalDbContext> options
) : DbContext(options) {
   public DbSet<Customer> Customers => Set<Customer>();
   public DbSet<Employee> Employess => Set<Employee>();
   public DbSet<Admin> Admins => Set<Admin>();
   public DbSet<Car> Cars => Set<Car>();
   public DbSet<Reservation> Reservations => Set<Reservation>();
   public DbSet<Rental> Rentals => Set<Rental>();

   protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      // Reuse one converter instance
      var dtOffMillis = new DateTimeOffsetToUnixTimeConverter();

      // TPT: Person base + derived tables
      //---------- Person Entity (base) ----------
      modelBuilder.Entity<Person>(b => {
         b.ToTable("Persons");

         b.HasKey(p => p.Id);
         b.Property(p => p.Id).ValueGeneratedNever();

         // Map common properties on Person (adjust names to your domain model)
         b.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
         b.Property(p => p.LastName).IsRequired().HasMaxLength(100);
         b.Property(p => p.Email).IsRequired().HasMaxLength(200);

         // If Address is a value object/owned type, map it here so it applies to all derived types
         b.OwnsOne(p => p.Address, a => {
            a.Property(x => x.Street).HasMaxLength(200);
            a.Property(x => x.PostalCode).HasMaxLength(20);
            a.Property(x => x.City).HasMaxLength(100);
         });
      });


      //---------- Employee Entity ----------
      modelBuilder.Entity<Employee>(b => {
         b.ToTable("Employees");
         // Employee-specific properties here (if any)
         b.Property(e => e.PersonnelNumber).IsRequired().HasMaxLength(16);
      });

      //---------- Admin Entity ----------
      modelBuilder.Entity<Admin>(b => {
         b.ToTable("Admins");
         // Admin-specific properties here (if any)
         b.Property(a => a.AdminRights).IsRequired();
      });

      modelBuilder.ApplyConfiguration(new ConfigCustomers());
      modelBuilder.ApplyConfiguration(new ConfigCars());
      modelBuilder.ApplyConfiguration(new ConfigReservations(dtOffMillis));
      modelBuilder.ApplyConfiguration(new ConfigRentals(dtOffMillis));
   }
}