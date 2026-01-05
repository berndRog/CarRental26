using CarRentalApi.Domain.Errors;
namespace CarRentalApi.Domain.UseCases.Cars;

public sealed class CarUcSendToMaintenance(
   ICarRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<CarUcSendToMaintenance> _logger
) : ICarUcSendToMaintenance {
   public async Task<Result> ExecuteAsync(Guid carId, CancellationToken ct) {

      // fetch car from database to repository
      var car = await _repository.FindByIdAsync(carId, ct);
      if (car is null) {
         _logger.LogWarning("CarUcSendToMaintenance rejected carId={id} not found", carId);
         return Result.Failure(CarErrors.NotFound);
      }

      // domain operation
      var result = car.SendToMaintenance();
      if (result.IsFailure) {
         _logger.LogWarning("CarUcSendToMaintenance rejected carId={id} errorCode={code}",
            carId, result.Error!.Code);
         return result;
      }

      // save changes to database
      await _unitOfWork.SaveAllChangesAsync("Car set to maintenance", ct);
      return Result.Success();
   }
}