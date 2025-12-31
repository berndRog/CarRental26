using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.ValueObjects;

namespace CarRentalApi.Domain.Service;

public interface IReservationConflictPolicy {
   Task<ReservationConflict> CheckAsync(
      CarCategory carCategory,
      RentalPeriod period,
      Guid ignoreReservationId,
      CancellationToken ct
   );
}
