using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.Entities;

public sealed class Car {
   
   public Guid Id { get; private set; }
   public string Manufacturer { get; private set; } = string.Empty;
   public string Model { get; private set; } = string.Empty;
   public string LicensePlate { get; private set; } = string.Empty;
   
   // Category is a enum used for booking and capacity calculation
   public CarCategory Category { get; private set; }
   public CarStatus Status { get; private set; }
   
   // EF Core ctor
   private Car() { }
   
   // Domain ctor
   public Car(
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
      
      // Validate category
      if (!Enum.IsDefined(typeof(CarCategory), category))
         return Result<Car>.Failure(CarErrors.CategoryIsRequired);

      // Validate manufacturer
      if (string.IsNullOrWhiteSpace(manufacturer))
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

      var result = EntityId.Resolve(id, CarErrors.InvalidId);
      if (result.IsFailure)
         return Result<Car>.Failure(result.Error);
      var carId = result.Value;

      return Result<Car>.Success(
         new Car(carId, category, manufacturer, model, licensePlate)
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
      if (Status == CarStatus.Retired)
         return Result.Failure(CarErrors.InvalidStatusTransition);

      Status = CarStatus.Maintenance;
      return Result.Success();
   }
}

