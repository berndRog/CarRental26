using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.UseCases.Reservations;

public sealed class ReservationUcExpire(
   IReservationRepository reservations,
   IUnitOfWork unitOfWork,
   ILogger<ReservationUcExpire> logger
): IReservationUcExpire {
   
   public async Task<Result<int>> ExecuteAsync(CancellationToken ct) {
      
      DateTimeOffset now = DateTimeOffset.UtcNow;
      logger.LogInformation("ReservationUcExpire start nowUtc={now}", now.ToDateTimeString());

      // fetch drafts to expire from repository or database
      var drafts = await reservations.SelectDraftsToExpireAsync(now, ct);
      logger.LogInformation("ReservationUcExpire found draftsToExpire={count}", drafts.Count);

      // domain logic to expire each draft reservation
      var expiredCount = 0;
      foreach (var reservation in drafts) {
         var result = reservation.Expire(now);
         if (result.IsFailure) {
            logger.LogWarning(
               "ReservationUcExpire skip reservationId={reservationId} errorCode={code} message={message}",
               reservation.Id, result.Error!.Code, result.Error!.Message);
            continue;
         }
         expiredCount++;
      }

      // unit of work to save all changes to database
      var saved = await unitOfWork.SaveAllChangesAsync("Expired reservation",ct);
      logger.LogInformation(
         "ReservationUcExpire done expiredCount={expiredCount} savedRows={saved}",
         expiredCount, saved);

      return Result<int>.Success(expiredCount);
   }
}
