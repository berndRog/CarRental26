using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;

namespace CarRentalApi.Domain.UseCases.Fleet;

public sealed class CarUcCreate(
   ICarRepository _cars,
   IUnitOfWork _uow,
   ILogger<CarUcCreate> _logger
) : ICarUcCreate {
   
   public async Task<Result<Car>> ExecuteAsync(
      CarCategory category,
      string manufacturer,
      string model,
      string licensePlate,
      string? id,
      CancellationToken ct
   ) {
      _logger.LogInformation(
         "CarUcCreate start category={cat} licensePlate={plate}",
         category, licensePlate
      );

      // Use-case rule: license plate must be unique.
      var exists = await _cars.ExistsLicensePlateAsync(licensePlate.Trim(), ct);
      if (exists)
         return Result<Car>.Failure(CarErrors.LicensePlateMustBeUnique);

      var result = Car.Create(category, manufacturer, model, licensePlate, id);
      if (result.IsFailure)
         return Result<Car>.Failure(result.Error!);

      _cars.Add(result.Value!);
      await _uow.SaveAllChangesAsync("Car added", ct);

      _logger.LogInformation("CarUcCreate done carId={id}", result.Value!.Id);
      return Result<Car>.Success(result.Value!);
   }
}

