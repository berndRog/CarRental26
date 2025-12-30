using CarRentalApi.Domain.Errors;
namespace CarRentalApi.Domain.UseCases.Reservations;

public sealed class ReservationUcConfirm(
   IReservationRepository _repository,
   IReservationConflictPolicy _conflicts,
   IUnitOfWork _unitOfWork,
   ILogger<ReservationUcConfirm> _logger
) : IReservationUcConfirm {
   
   public async Task<Result> ExecuteAsync(
      Guid reservationId, 
      CancellationToken ct
   ) {
      _logger.LogInformation(
         "ReservationUcConfirm start reservationId={id}", reservationId);

      // fetch from repository or query database
      var reservation = await _repository.FindByIdAsync(reservationId, ct);

      if (reservation is null) {
         _logger.LogWarning(
            "ReservationUcConfirm rejected reservationId={id} errorCode={code}",
            reservationId, ReservationErrors.NotFound.Code);
         return Result.Failure(ReservationErrors.NotFound);
      }

      // domain model
      var result = await reservation.ConfirmAsync(_conflicts, DateTimeOffset.UtcNow, ct);
      if (result.IsFailure) {
         _logger.LogWarning(
            "ReservationUcConfirm rejected reservationId={id} errorCode={code}",
            reservationId, result.Error!.Code);
         return result;
      }

      // unit of work: save changes in reservation to database
      await _unitOfWork.SaveAllChangesAsync("Resvation confirmed",ct);

      _logger.LogInformation(
         "ReservationUcConfirm done reservationId={id}", reservationId);

      return Result.Success();
   }
}