using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.Errors;

public static class RentalErrors {
   public static readonly DomainErrors InvalidId =
      new("rental.invalid_id", "RentalContract id is invalid.");

   public static readonly DomainErrors NotFound =
      new("rental.not_found", "RentalContract not found.");

   public static readonly DomainErrors InvalidStatusTransition =
      new("rental.invalid_status_transition", "CarStatus transition is not allowed.");

   public static readonly DomainErrors InvalidTimestamp =
      new("rental.invalid_timestamp", "Timestamp is invalid.");

   public static readonly DomainErrors InvalidFuelLevel =
      new("rental.invalid_fuel_level", "Fuel level must be between 0 and 100.");

   public static readonly DomainErrors InvalidKm =
      new("rental.invalid_odometer", "km value is invalid.");

   public static readonly DomainErrors InvalidReservation =
      new("rental.invalid_reservation", "Reservation reference is invalid.");

   public static readonly DomainErrors InvalidCar =
      new("rental.invalid_car", "Car reference is invalid.");

   public static readonly DomainErrors InvalidCustomer =
      new("rental.invalid_customer", "Customer reference is invalid.");
}