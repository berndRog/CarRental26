using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Service;
using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApiTest.Domain.Entites;

public sealed class ReservationUt {

   private TestSeed _seed = new TestSeed();
   
   private sealed class NoConflictPolicy : IReservationConflictPolicy {
      public bool HasConflict(Guid vehicleId, RentalPeriod period, Guid excludingReservationId) => false;
   }

   private sealed class ConflictPolicy : IReservationConflictPolicy {
      public bool HasConflict(Guid vehicleId, RentalPeriod period, Guid excludingReservationId) => true;
   }

   
   // [Fact]
   // public void Expire_only_allowed_in_draft() {
   //    var period = RentalPeriod.Create(
   //       new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
   //       new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)).Value!;
   //
   //    var reservation = Reservation.CreateDraft(
   //       Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), period,
   //       new DateTimeOffset(2025, 12, 27, 0, 0, 0, TimeSpan.Zero));
   //
   //    reservation.Confirm(new NoConflictPolicy(),
   //       new DateTimeOffset(2025, 12, 27, 1, 0, 0, TimeSpan.Zero));
   //
   //    var result = reservation.Expire(new DateTimeOffset(2025, 12, 27, 2, 0, 0, TimeSpan.Zero));
   //
   //    Assert.True(result.IsFailure);
   //    Assert.Equal(ReservationErrors.InvalidStatusTransition.Code, result.Error!.Code);
   // }
}