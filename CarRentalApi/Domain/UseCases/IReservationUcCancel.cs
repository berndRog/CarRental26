namespace CarRentalApi.Domain.UseCases;

public interface IReservationUcCancel {
   Task<Result> ExecuteAsync(
      Guid reservationId,
      CancellationToken ct = default
   );
}