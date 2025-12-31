using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain.UseCases;

public interface ICarUcCreate {
   Task<Result<Car>> ExecuteAsync(
      CarCategory category,
      string manufacturer,
      string model,
      string licensePlate,
      string? id,
      CancellationToken ct
   );
}