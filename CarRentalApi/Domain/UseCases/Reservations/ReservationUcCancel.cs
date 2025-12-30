using CarRentalApi.Domain.Errors;
namespace CarRentalApi.Domain.UseCases.Reservations;

public sealed class ReservationUcCancel(
   IReservationRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<ReservationUcCancel> _logger
): IReservationUcCancel {

   public async Task<Result> ExecuteAsync(
      Guid reservationId, 
      CancellationToken ct
   ) {
      _logger.LogInformation(
         "ReservationUcCancel start reservationId={reservationId}",
         reservationId);

      // fetch from repository or query database
      var reservation = await _repository.FindByIdAsync(reservationId, ct);
      if (reservation is null) {
         _logger.LogWarning(
            "ReservationUcCancel rejected reservationId={reservationId} errorCode={code}",
            reservationId, ReservationErrors.NotFound.Code);
         return Result.Failure(ReservationErrors.NotFound);
      }
      
      // domain model logic
      var result = reservation.Cancel(DateTimeOffset.UtcNow);
      if (result.IsFailure) {
         _logger.LogWarning(
            "ReservationUcCancel rejected reservationId={reservationId} errorCode={code} message={message}",
            reservationId, result.Error!.Code, result.Error!.Message);
         return result;
      }

      // unit of work to save all changes to database
      var saved = await _unitOfWork.SaveAllChangesAsync("", ct);
      _logger.LogInformation(
         "ReservationUcCancel done reservationId={reservationId} savedRows={saved}",
         reservationId, saved);
      return Result.Success();
   }
}