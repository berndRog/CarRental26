using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApi.Domain.Service;

public interface IReservationConflictPolicy {
   bool HasConflict(
      Guid vehicleId,
      RentalPeriod period,
      Guid excludingReservationId);
}
