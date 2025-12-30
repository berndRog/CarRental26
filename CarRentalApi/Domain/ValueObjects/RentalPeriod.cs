using CarRentalApi.Domain.Errors;
namespace CarRentalApi.Domain.ValueObjects;

public sealed record class RentalPeriod(
   DateTimeOffset Start,
   DateTimeOffset End
) {
   public static Result<RentalPeriod> Create(
      DateTimeOffset start,
      DateTimeOffset end
   ) {
      // invariant end > start
      if (start >= end)
         return Result<RentalPeriod>.Failure(ReservationErrors.InvalidPeriod);

      return Result<RentalPeriod>.Success(new RentalPeriod(start, end));
   }

   public bool Overlaps(RentalPeriod other) =>
      Start < other.End && other.Start < End;
}