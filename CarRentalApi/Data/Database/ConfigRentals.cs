using CarRentalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CarRentalApi.Data.Database.Configurations;

public sealed class ConfigRentals(
   DateTimeOffsetToUnixTimeConverter _dtOffMillis
) : IEntityTypeConfiguration<Rental> {
   
   public void Configure(EntityTypeBuilder<Rental> b) {
      b.ToTable("Rentals");
      b.HasKey(x => x.Id);
      b.Property(x => x.Id)
         .ValueGeneratedNever();

      // References
      b.Property(x => x.ReservationId).IsRequired();
      b.Property(x => x.CustomerId).IsRequired();
      b.Property(x => x.CarId).IsRequired();

      // Status as int
      b.Property(x => x.Status)
         .HasConversion<int>()
         .IsRequired();

      // Pick-up
      b.Property(x => x.PickupAt)
         .IsRequired()
         .HasConversion(_dtOffMillis);
      b.Property(x => x.FuelLevelOut)
         .IsRequired();
      b.Property(x => x.KmOut)
         .IsRequired();

      // Return (nullable)
      b.Property(x => x.ReturnAt)
         .IsRequired(false)
         .HasConversion(_dtOffMillis);
      b.Property(x => x.FuelLevelIn)
         .IsRequired(false);
      b.Property(x => x.KmIn)
         .IsRequired(false);

      // Helpful indexes (fast lookups)
      b.HasIndex(x => x.CarId);
      b.HasIndex(x => x.ReservationId);
      b.HasIndex(x => new { x.CarId, x.Status });
      b.HasIndex(x => new { x.ReservationId, x.Status });

      // NOTE:
      // We don't define navigations here (kept minimal).
      // If you later add navigation properties, configure FK relationships here.
   }
}