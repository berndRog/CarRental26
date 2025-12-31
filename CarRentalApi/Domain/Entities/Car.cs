using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;

namespace CarRentalApi.Domain.Entities;

public sealed class Car {
   public Guid Id { get; private set; }
   public string Manufacturer { get; private set; } = string.Empty;
   public string Model { get; private set; } = string.Empty;
   public string LicensePlate { get; private set; } = string.Empty;

   // Category is used for booking and capacity calculation.
   public CarCategory Category { get; private set; }
   public CarStatus Status { get; private set; }

   // EF Core ctor
   private Car() {
   }

   // Domain ctor
   private Car(
      Guid id,
      CarCategory category,
      string manufacturer,
      string model,
      string licensePlate
   ) {
      Id = id;
      Category = category;
      Manufacturer = manufacturer;
      Model = model;
      LicensePlate = licensePlate;
      Status = CarStatus.Available;
   }

   // ---------- Factory (Result-based) ----------
   public static Result<Car> Create(
      CarCategory category,
      string manufacturer,
      string model,
      string licensePlate,
      string? id = null
   ) {
      // Normalize input early
      manufacturer = manufacturer?.Trim() ?? string.Empty;
      model = model?.Trim() ?? string.Empty;
      licensePlate = licensePlate?.Trim() ?? string.Empty;

      // Validate category
      if (!Enum.IsDefined(typeof(CarCategory), category))
         return Result<Car>.Failure(CarErrors.CategoryIsRequired);

      // Validate manufacturer
      if (string.IsNullOrWhiteSpace(manufacturer))
         return Result<Car>.Failure(CarErrors.ManufacturerIsRequired);

      // Validate model
      if (string.IsNullOrWhiteSpace(model))
         return Result<Car>.Failure(CarErrors.ModelIsRequired);

      // Validate license plate
      if (string.IsNullOrWhiteSpace(licensePlate))
         return Result<Car>.Failure(CarErrors.LicensePlateIsRequired);

      var idResult = EntityId.Resolve(id, CarErrors.InvalidId);
      if (idResult.IsFailure)
         return Result<Car>.Failure(idResult.Error);

      return Result<Car>.Success(
         new Car(idResult.Value, category, manufacturer, model, licensePlate)
      );
   }

   // ---------- Domain behavior ----------
   public Result MarkAsRented() {
      if (Status != CarStatus.Available)
         return Result.Failure(CarErrors.CarNotAvailable);

      Status = CarStatus.Rented;
      return Result.Success();
   }

   public Result MarkAsAvailable() {
      if (Status != CarStatus.Rented)
         return Result.Failure(CarErrors.InvalidStatusTransition);

      Status = CarStatus.Available;
      return Result.Success();
   }

   public Result SendToMaintenance() {
      // Business rule: retired cars cannot change status anymore.
      if (Status == CarStatus.Retired)
         return Result.Failure(CarErrors.InvalidStatusTransition);

      Status = CarStatus.Maintenance;
      return Result.Success();
   }

   public Result Retire() {
      // Business rule: a car can be retired from any state except already retired.
      if (Status == CarStatus.Retired)
         return Result.Success();

      Status = CarStatus.Retired;
      return Result.Success();
   }
}