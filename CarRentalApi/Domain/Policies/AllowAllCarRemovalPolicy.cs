namespace CarRentalApi.Domain.Service;

public sealed class AllowAllCarRemovalPolicy : ICarRemovalPolicy {
   public Task<bool> CheckAsync(Guid carId, CancellationToken ct) =>
      Task.FromResult(true);
}
