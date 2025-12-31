using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;
using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApi.Domain.Entities;

public sealed class Reservation {
   public Guid Id { get; private set; }
   public CarCategory CarCategory { get; private set; }
   public Guid CustomerId { get; private set; }

   public RentalPeriod Period { get; private set; } = default!;
   public ReservationStatus Status { get; private set; }

   public DateTimeOffset CreatedAt { get; private set; }
   public DateTimeOffset? ConfirmedAt { get; private set; }
   public DateTimeOffset? CancelledAt { get; private set; }
   public DateTimeOffset? ExpiredAt { get; private set; }

   // EF Core
   private Reservation() { }

   // Domain ctor
   private Reservation(
      Guid id,
      Guid customerId,
      CarCategory carCategory,
      RentalPeriod period,
      DateTimeOffset createdAt
   ) {
      Id = id;
      CustomerId = customerId;
      CarCategory = carCategory;
      Period = period;

      Status = ReservationStatus.Draft;
      CreatedAt = createdAt;
   }

   // ---------- Factory (Result-based) ----------
   // NOTE:
   // - This method enforces only domain invariants.
   // - Context-specific rules (e.g. start > now) belong to the UseCase.
   public static Result<Reservation> CreateDraft(
      Guid customerId,
      CarCategory carCategory,
      DateTimeOffset start,
      DateTimeOffset end,
      DateTimeOffset createdAt,
      string? id = null
   ) {
      var idResult = EntityId.Resolve(id, ReservationErrors.InvalidId);
      if (idResult.IsFailure)
         return Result<Reservation>.Failure(idResult.Error);
      var reservationId = idResult.Value;

      var periodResult = RentalPeriod.Create(start, end);
      if (periodResult.IsFailure)
         return Result<Reservation>.Failure(periodResult.Error);
      var period = periodResult.Value;

      return Result<Reservation>.Success(
         new Reservation(reservationId, customerId, carCategory, period, createdAt)
      );
   }

   // ---------- Domain Behavior (Result-based) ----------
   public Result ChangePeriod(DateTimeOffset newStart, DateTimeOffset newEnd) {
      // Only draft reservations can be modified.
      if (Status != ReservationStatus.Draft)
         return Result.Failure(ReservationErrors.InvalidStatusTransition);

      var periodResult = RentalPeriod.Create(newStart, newEnd);

      if (periodResult.IsFailure) {
         // Ensure the compiler sees: error is not null here
         var error = periodResult.Error ?? ReservationErrors.InvalidPeriod;
         return Result.Failure(error);
      }

      // Ensure the compiler sees: value is not null here
      var period = periodResult.Value ?? throw new InvalidOperationException(
         "RentalPeriod.Create returned success with null Value.");

      Period = period;
      return Result.Success();
   }

   public Result Confirm(DateTimeOffset confirmedAt) {
      // Only draft reservations can be confirmed.
      if (Status != ReservationStatus.Draft)
         return Result.Failure(ReservationErrors.InvalidStatusTransition);

      // Domain consistency: confirmation cannot happen before creation.
      if (confirmedAt < CreatedAt)
         return Result.Failure(ReservationErrors.InvalidTimestamp);

      Status = ReservationStatus.Confirmed;
      ConfirmedAt = confirmedAt;
      return Result.Success();
   }

   public Result Cancel(DateTimeOffset cancelledAt) {
      // Only draft or confirmed reservations can be cancelled.
      if (Status is not (ReservationStatus.Draft or ReservationStatus.Confirmed))
         return Result.Failure(ReservationErrors.InvalidStatusTransition);

      // Domain consistency: cancellation cannot happen before creation.
      if (cancelledAt < CreatedAt)
         return Result.Failure(ReservationErrors.InvalidTimestamp);

      Status = ReservationStatus.Cancelled;
      CancelledAt = cancelledAt;
      return Result.Success();
   }

   public Result Expire(DateTimeOffset expiredAt) {
      // Only draft reservations can expire.
      if (Status != ReservationStatus.Draft)
         return Result.Failure(ReservationErrors.InvalidStatusTransition);

      // Domain consistency: expiration cannot happen before creation.
      if (expiredAt < CreatedAt)
         return Result.Failure(ReservationErrors.InvalidTimestamp);

      Status = ReservationStatus.Expired;
      ExpiredAt = expiredAt;
      return Result.Success();
   }

   // Map policy result to a domain error (call only when conflict != None).
   public static DomainErrors MapConflict(ReservationConflict conflict) =>
      conflict switch {
         ReservationConflict.NoCategoryCapacity => ReservationErrors.NoCarCategoryCapacity,
         ReservationConflict.OverCapacity => ReservationErrors.Conflict,
         _ => throw new InvalidOperationException("MapConflict called with no conflict.")
      };
}