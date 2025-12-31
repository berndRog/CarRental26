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
      
      // Domain invariant:
      // A rental period must have a positive duration.
      if (start >= end)
         return Result<RentalPeriod>.Failure(ReservationErrors.InvalidPeriod);

      return Result<RentalPeriod>.Success(new RentalPeriod(start, end));
   }

   // Half-open interval overlap check:
   // [Start, End) overlaps [other.Start, other.End)
   public bool Overlaps(RentalPeriod other) =>
      Start < other.End && other.Start < End;
}
