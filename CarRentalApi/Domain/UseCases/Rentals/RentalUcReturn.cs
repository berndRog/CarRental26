using CarRentalApi.Domain.Entities;
namespace CarRentalApi.Domain.UseCases.Rentals;

public sealed class RentalUcReturn(
   IRentalRepository _rentalRepo,
   IUnitOfWork _unitOfWork,
   IClock _clock,
   ILogger<RentalUcReturn> _logger
) : IRentalUcReturn {
   
   public async Task<Result<Rental>> ExecuteAsync(
      Guid rentalId,
      int fuelLevelIn,
      int kmIn,
      CancellationToken ct
   ) {

      // fetch rental from database and track it (via EF Core DbContext)
      var rental = await _rentalRepo.FindByIdAsync(rentalId, ct);
      if (rental is null)
         return Result<Rental>.Failure(RentalUcErrors.RentalNotFound);

      // domain model operation
      var returnAt = _clock.UtcNow;
      var result = rental.ReturnCar(
         returnAt: returnAt,
         fuelLevelIn: fuelLevelIn,
         kmIn: kmIn
      );
      if (result.IsFailure)
         return Result<Rental>.Failure(result.Error);

      // no update method needed if using EF Core tracking
      // _rentalRepo.Update(rental);

      var saved = await _unitOfWork.SaveAllChangesAsync("RentalUcReturn", ct);
      if (!saved)
         return Result<Rental>.Failure(RentalUcErrors.RentalSaveFailed);

      // Optional: Gebühren/Policies nur als Hinweis (gehört oft in separaten Policy/Service)
      // var needsRefuelFee = rental.NeedsRefuelFee();

      _logger.LogInformation(
         "RentalUcReturn success rentalId={rentalId} returned={returned} needsRefuelFee={needsRefuelFee}",
         rental.Id, rental.IsReturned(), rental.NeedsRefuelFee()
      );
      return Result<Rental>.Success(rental);
   }
}