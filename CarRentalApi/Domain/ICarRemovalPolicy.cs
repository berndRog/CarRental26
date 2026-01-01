namespace CarRentalApi.Domain;

public interface ICarRemovalPolicy {
   Task<bool> CheckAsync(Guid carId, CancellationToken ct);
}
