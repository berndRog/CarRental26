namespace CarRentalApi.Domain.UseCases;

public interface IReservationUcConfirm {
   Task<Result> ExecuteAsync(
      Guid reservationId,
      CancellationToken ct = default
   );
}