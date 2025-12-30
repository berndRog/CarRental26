using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApi.Domain.Policies;

public sealed class ReservationConflictPolicy(
   IReservationRepository _reservations,
   ICarRepository _cars
) : IReservationConflictPolicy {
   
   public async Task<bool> HasConflictAsync(
      CarCategory category,
      RentalPeriod period,
      Guid ignoreReservationId,
      CancellationToken ct
   ) {
      var capacity =
         await _cars.CountCarsInCategoryAsync(category, ct);

      if (capacity <= 0)
         return true;

      var overlapping =
         await _reservations.CountConfirmedOverlappingAsync(
            category, period.Start, period.End, ignoreReservationId, ct
         );

      return overlapping >= capacity;
   }


}

