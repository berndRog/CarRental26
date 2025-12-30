using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApi.Domain;

public interface IReservationConflictPolicy {
   Task<bool> HasConflictAsync(
      CarCategory carCategory,
      RentalPeriod period,
      Guid ignoreReservationId,
      CancellationToken ct
   );
}
