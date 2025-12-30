namespace CarRentalApi.Domain.UseCases;

public interface IReservationUcExpire {
   Task<Result<int>> ExecuteAsync(
      CancellationToken ct = default
   );
}