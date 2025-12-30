namespace CarRentalApi.Domain.Errors;

public static class CarErrors {
   public static readonly DomainErrors InvalidId = new("car.invalid_id",
      Message: "The provided car id is invalid."
   );
   
   public static readonly DomainErrors CategoryIsRequired = new("car.category_required",
         "Category is required."); 
   
   public static readonly DomainErrors ManufacturerIsRequired = new("car.manufacturer_required",
         "Manufacturer must not be empty.");

   public static readonly DomainErrors ModelIsRequired = new("car.model_required",
         "Model must not be empty.");

   public static readonly DomainErrors LicensePlateIsRequired = new("car.license_plate_required",
         "License plate must not be empty.");
   
   public static readonly DomainErrors CarNotAvailable = new("car.not_available",
         Message: "Car is not available.");

   public static readonly DomainErrors InvalidStatusTransition = new("car.invalid_status_transition",
         Message: "Invalid car status transition.");
}
