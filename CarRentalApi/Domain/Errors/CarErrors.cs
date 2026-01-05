namespace CarRentalApi.Domain.Errors;

public static class CarErrors {
   public static readonly DomainErrors InvalidId =
      new("car.invalid_id", "Car id is invalid.");

   public static readonly DomainErrors CategoryIsRequired =
      new("car.category_required", "Car category is required.");

   public static readonly DomainErrors ManufacturerIsRequired =
      new("car.manufacturer_required", "Manufacturer is required.");

   public static readonly DomainErrors ModelIsRequired =
      new("car.model_required", "Model is required.");

   public static readonly DomainErrors LicensePlateIsRequired =
      new("car.license_plate_required", "License plate is required.");

   public static readonly DomainErrors InvalidLicensePlateFormat = 
      new("Car.InvalidLicensePlateFormat",
      "License plate can only contain uppercase letters, digits and hyphens");
   
   public static readonly DomainErrors LicensePlateMustBeUnique =
      new("car.license_plate_unique", "License plate must be unique.");

   public static readonly DomainErrors CarNotAvailable =
      new("car.not_available", "Car is not available.");

   public static readonly DomainErrors InvalidStatusTransition =
      new("car.invalid_status_transition", "CarStatus transition is not allowed.");

   public static readonly DomainErrors NotFound =
      new("car.not_found", "Car not found.");
}