using CarRentalApi.Domain.Entities;
namespace CarRentalApi.Domain.UseCases;

public interface IRentalUcReturn {
   Task<Result<Rental>> ExecuteAsync(
      Guid rentalId,
      int fuelLevelIn,
      int kmIn,
      CancellationToken ct
   );
}