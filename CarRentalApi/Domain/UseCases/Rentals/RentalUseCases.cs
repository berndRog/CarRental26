namespace CarRentalApi.Domain.UseCases.Rentals;

public sealed class RentalUseCases(
   IRentalUcPickup pickup,
   IRentalUcReturn @return
) : IRentalUseCases
{
   public IRentalUcPickup Pickup { get; } = pickup;
   public IRentalUcReturn Return { get; } = @return;
}
