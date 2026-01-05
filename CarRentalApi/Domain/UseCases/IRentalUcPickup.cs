using CarRentalApi.Domain.Entities;
namespace CarRentalApi.Domain.UseCases;

public interface IRentalUcPickup {
   Task<Result<Rental>> ExecuteAsync(
      Guid reservationId,
      Guid customerId,
      Guid carId,
      int fuelLevelOut,
      int kmOut,
      CancellationToken ct
   );
}