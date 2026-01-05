using CarRentalApi.Domain.Errors;

namespace CarRentalApi.Domain.UseCases.Rentals;

public static class RentalUcErrors {
   public static readonly DomainErrors ReservationNotFound =
      new("RentalUc.ReservationNotFound", "Reservation not found.");
   public static readonly DomainErrors CustomerNotFound    = 
      new("RentalUc.CustomerNotFound", "Customer not found.");
   public static readonly DomainErrors CarNotFound         = 
      new("RentalUc.CarNotFound", "Car not found.");
   public static readonly DomainErrors RentalNotFound      = 
      new("RentalUc.RentalNotFound", "Rental not found.");

   public static readonly DomainErrors ReservationInvalidStatus =
      new("RentalUc.ReservationInvalidStatus", "Reservation status does not allow pick-up.");
   public static readonly DomainErrors RentalSaveFailed =
      new("RentalUc.SaveFailed", "Could not persist pick-up.");

}
