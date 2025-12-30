using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Service;
using CarRentalApi.Domain.Utils;
using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApi.Domain.Entities;

public sealed class Reservation {
    
    public Guid Id { get; private set; }
    public CarCategory CarCategory { get; private set; }
    public Guid CustomerId { get; private set; }

    public RentalPeriod Period { get; private set; }  =
        new(DateTimeOffset.MinValue, DateTimeOffset.MinValue); // Required for EF materialization
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
    public static Result<Reservation> CreateDraft(
        Guid customerId,
        CarCategory carCategory,
        RentalPeriod period,
        DateTimeOffset createdAt,
        string? id = null
    ) {
        var result = EntityId.Resolve(id, ReservationErrors.InvalidId);
        if (result.IsFailure)
            return Result<Reservation>.Failure(result.Error);
        var reservationId = result.Value;
        
        return Result<Reservation>.Success(
            new Reservation(reservationId, customerId, carCategory, period, createdAt)
        );
    }

    // ---------- Domain Behavior (Result-based) ----------
    public Result ChangePeriod(RentalPeriod newPeriod) {
        if (Status != ReservationStatus.Draft)
            return Result.Failure(ReservationErrors.InvalidStatusTransition);

        Period = newPeriod;
        return Result.Success();
    }

    public async Task<Result> ConfirmAsync(
        IReservationConflictPolicy conflictPolicy,
        DateTimeOffset confirmedAt,
        CancellationToken ct
    ) {
        if (Status != ReservationStatus.Draft)
            return Result.Failure(ReservationErrors.InvalidStatusTransition);
        
        if (Period.Start >= Period.End) 
            return Result.Failure(ReservationErrors.InvalidPeriod);

        var hasConflict = await conflictPolicy.HasConflictAsync(
            carCategory: CarCategory,
            period: Period,
            ignoreReservationId: Id,
            ct: ct
        );

        if (hasConflict)
            return Result.Failure(ReservationErrors.Conflict); // oder NoCarCategoryCapacity

        Status = ReservationStatus.Confirmed;
        ConfirmedAt = confirmedAt;
        return Result.Success();
    }


    public Result Cancel(
        DateTimeOffset cancelledAt
    ) {
        if (Status is ReservationStatus.Cancelled or ReservationStatus.Expired)
            return Result.Failure(ReservationErrors.InvalidStatusTransition);

        Status = ReservationStatus.Cancelled;
        CancelledAt = cancelledAt;
        return Result.Success();
    }

    public Result Expire(
        DateTimeOffset expiredAt
    ) {
        if (Status != ReservationStatus.Draft)
            return Result.Failure(ReservationErrors.InvalidStatusTransition);

        Status = ReservationStatus.Expired;
        ExpiredAt = expiredAt;
        return Result.Success();
    }
}
