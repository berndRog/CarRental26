using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CarRentalApi.Domain.UseCases.Reservations;

public sealed class ReservationUcCreateDraft (
      IReservationRepository reservations,
      IUnitOfWork unitOfWork,
      ILogger<ReservationUcCreateDraft> logger
): IReservationUcCreateDraft {

   public async Task<Result<Reservation>> ExecuteAsync(
      Guid customerId,
      CarCategory carCategory,
      DateTimeOffset start,
      DateTimeOffset end,
      string? id,
      CancellationToken ct
   ) {
      logger.LogInformation(
         "ReservationUcDraft start customerId={Id} carCategory {cat}, start={start} end={end}",
         customerId, carCategory, start, end);

      // domain
      var now = DateTimeOffset.UtcNow;
      // invariant start > now
      if (start <= now)
         return Result<Reservation>.Failure(ReservationErrors.StartDateInPast);
      
      var resultPeriod = RentalPeriod.Create(start, end);
      if (resultPeriod.IsFailure) {
         logger.LogWarning(
            "ReservationUcDraft rejected errorCode={code} message={message}",
            resultPeriod.Error!.Code, resultPeriod.Error!.Message);
         return Result<Reservation>.Failure(resultPeriod.Error!);
      }
      var period = resultPeriod.Value!;

      var result = Reservation.CreateDraft(
         carCategory: carCategory,
         customerId: customerId,
         period: period,
         createdAt: now, 
         id : id
      );
      if (result.IsFailure) {
         logger.LogWarning(
            "ReservationUcDraft rejected errorCode={code} message={message}",
            result.Error!.Code, result.Error!.Message);
         return Result<Reservation>.Failure(result.Error!);
      }
      var reservation = result.Value!;

      // add reservation
      reservations.Add(reservation);

      // unit of Work: save changes to database
      var saved = await unitOfWork.SaveAllChangesAsync("Reservation added",ct);
      
      logger.LogInformation(
         "ReservationUcDraft done reservationId={reservationId} savedRows={saved}",
         reservation.Id, saved);

      return Result<Reservation>.Success(reservation);
   }
}
