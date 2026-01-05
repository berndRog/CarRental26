namespace CarRentalApi.Domain.UseCases.Reservations;

public interface IReservationUcChangePeriod {
   Task<Result> ExecuteAsync(
      Guid reservationId, 
      DateTimeOffset newStart, 
      DateTimeOffset newEnd, 
      CancellationToken ct
   );
}
