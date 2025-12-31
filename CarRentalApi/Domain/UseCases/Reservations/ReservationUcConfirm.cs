using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Service;
namespace CarRentalApi.Domain.UseCases.Reservations;

public sealed class ReservationUcConfirm(
   IReservationRepository _repository,
   IUnitOfWork _unitOfWork,
   IReservationConflictPolicy _conflicts,
   ILogger<ReservationUcConfirm> _logger,
   IClock _clock
) : IReservationUcConfirm {
   
   public async Task<Result> ExecuteAsync(Guid reservationId, CancellationToken ct) {
      _logger.LogInformation(
         "ReservationUcConfirm start reservationId={id}",
         reservationId
      );

      // Fetch reservation (aggregate) from _repository.
      var reservation = await _repository.FindByIdAsync(reservationId, ct);
      if (reservation is null) {
         _logger.LogWarning(
            "ReservationUcConfirm rejected reservationId={id} errorCode={code}",
            reservationId, ReservationErrors.NotFound.Code
         );
         return Result.Failure(ReservationErrors.NotFound);
      }
      
      // Use-case / domain-service rule:
      // Check category capacity for the requested period.
      var now = _clock.UtcNow;
      var conflict = await _conflicts.CheckAsync(
         carCategory: reservation.CarCategory,
         period: reservation.Period,
         ignoreReservationId: reservation.Id,
         ct: ct
      );

      if (conflict != ReservationConflict.None) {
         var error = Reservation.MapConflict(conflict);

         _logger.LogWarning(
            "ReservationUcConfirm rejected reservationId={id} conflict={conflict} errorCode={code}",
            reservationId, conflict, error.Code);

         return Result.Failure(error);
      }

      // Domain transition (pure, no I/O).
      var result = reservation.Confirm(now);
      if (result.IsFailure) {
         _logger.LogWarning(
            "ReservationUcConfirm rejected reservationId={id} errorCode={code}",
            reservationId, result.Error!.Code
         );
         return result;
      }

      // Persist updated state.
      await _unitOfWork.SaveAllChangesAsync("Reservation confirmed", ct);

      _logger.LogDebug("ReservationUcConfirm done reservationId={id}",
         reservationId);

      return Result.Success();
   }
}
