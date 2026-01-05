using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain.UseCases.Rentals;

public sealed class RentalUcPickup(
   IReservationRepository _reservationRepo,
   ICustomerRepository _customerRepo,
   ICarRepository _carRepo,
   IRentalRepository _rentalRepo,
   IUnitOfWork _unitOfWork,
   IClock _clock,
   ILogger<RentalUcPickup> _logger
) : IRentalUcPickup {
   public async Task<Result<Rental>> ExecuteAsync(
      Guid reservationId,
      Guid customerId,
      Guid carId,
      int fuelLevelOut,
      int kmOut,
      CancellationToken ct
   ) {
      _logger.LogInformation(
         "RentalUcPickup start reservationId={reservationId} customerId={customerId} carId={carId}",
         reservationId, customerId, carId
      );

      // --- Load & existence checks (DDD via repos) ---
      var reservation = await _reservationRepo.FindByIdAsync(reservationId, ct);
      if (reservation is null)
         return Result<Rental>.Failure(RentalUcErrors.ReservationNotFound);

      var customer = await _customerRepo.FindByIdAsync(customerId, ct);
      if (customer is null)
         return Result<Rental>.Failure(RentalUcErrors.CustomerNotFound);

      var car = await _carRepo.FindByIdAsync(carId, ct);
      if (car is null)
         return Result<Rental>.Failure(RentalUcErrors.CarNotFound);

      if (reservation.ResStatus != ReservationStatus.Confirmed)
          return Result<Rental>.Failure(RentalUcErrors.ReservationInvalidStatus);

      var pickupAt = _clock.UtcNow;

      // --- Create Rental aggregate root ---
      var rentalResult = Rental.CreateAtPickup(
         reservationId: reservationId,
         customerId: customerId,
         carId: carId,
         pickupAt: pickupAt,
         fuelLevelOut: fuelLevelOut,
         kmOut: kmOut
      );

      if (rentalResult.IsFailure)
         return Result<Rental>.Failure(rentalResult.Error);

      var rental = rentalResult.Value;
      _rentalRepo.Add(rental);

      var saved = await _unitOfWork.SaveAllChangesAsync("RentalUcPickup", ct);
      if (!saved)
         return Result<Rental>.Failure(RentalUcErrors.RentalSaveFailed);

      _logger.LogInformation("RentalUcPickup success rentalId={rentalId}", rental.Id);
      return Result<Rental>.Success(rental);
   }
}