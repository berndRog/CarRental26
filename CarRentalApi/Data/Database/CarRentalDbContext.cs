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
      var dtOffMillis = new DateTimeOffsetToIsoStringConverter();

      // TPT: Person base + derived tables
      modelBuilder.Ignore<Entity<Guid>>();
      // Entity Person -> Table People
      modelBuilder.ApplyConfiguration(new ConfigPeople());
      // Entity Customer -> Table Customers
      modelBuilder.ApplyConfiguration(new ConfigCustomers());
      // Entity Employee -> Table Employees
      modelBuilder.ApplyConfiguration(new ConfigEmployees());
      // Entity Admin -> Table Admins
      modelBuilder.ApplyConfiguration(new ConfigAdmins());
      
      // Entity Car -> Table Cars
      modelBuilder.ApplyConfiguration(new ConfigCars());
      // Entity Reservation -> Table Reservations
      modelBuilder.ApplyConfiguration(new ConfigReservations(dtOffMillis));
      // Entity Rental -> Table Rentals
      modelBuilder.ApplyConfiguration(new ConfigRentals(dtOffMillis));
   }
}