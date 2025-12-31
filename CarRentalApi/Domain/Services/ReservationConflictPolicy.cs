using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.ValueObjects;

namespace CarRentalApi.Domain.Service;

public sealed class ReservationConflictPolicy(
   IReservationRepository _reservationRepository,
   ICarRepository _carRepository
) : IReservationConflictPolicy {
   
   public async Task<ReservationConflict> CheckAsync(
      CarCategory carCategory,
      RentalPeriod period,
      Guid ignoreReservationId,
      CancellationToken ct
   ) {
      var capacity = await _carRepository.CountCarsInCategoryAsync(carCategory, ct);
      if (capacity <= 0)
         return ReservationConflict.NoCategoryCapacity;

      var overlapping = await _reservationRepository.CountConfirmedOverlappingAsync(
         carCategory,
         period.Start,
         period.End,
         ignoreReservationId,
         ct
      );

      return overlapping >= capacity
         ? ReservationConflict.OverCapacity
         : ReservationConflict.None;
   }
}

