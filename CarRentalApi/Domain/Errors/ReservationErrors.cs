namespace CarRentalApi.Domain.Errors;

public static class ReservationErrors {
   
   public static readonly DomainErrors InvalidId = new("reservation.invalid_id",
      "Reservation id is invalid.");
   
   public static readonly DomainErrors NotFound = new("reservation.not_found",
      "Reservation not found.");

   public static readonly DomainErrors InvalidStatusTransition = new("reservation.invalid_status_transition", 
      "Status transition is not allowed.");

   public static readonly DomainErrors Conflict = new("reservation.conflict", 
      "Vehicle is already reserved in the selected period.");
   
   public static readonly DomainErrors StartDateInPast = new("reservation.start_date_in_past", 
      "Reservation start must be in the future.");

   public static readonly DomainErrors InvalidPeriod = new("reservation.invalid_period",
         "Reservation period is invalid: start must be earlier than end.");
   
   public static readonly DomainErrors NoCarCategoryCapacity = new("reservation.no_car_category_capacity",
         "No cars available in the selected category for the given period.");

}

