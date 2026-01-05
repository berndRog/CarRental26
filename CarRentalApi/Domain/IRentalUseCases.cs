namespace CarRentalApi.Domain.UseCases.Rentals;

public interface IRentalUseCases {
   IRentalUcPickup Pickup { get; }
   IRentalUcReturn Return { get; }
}
