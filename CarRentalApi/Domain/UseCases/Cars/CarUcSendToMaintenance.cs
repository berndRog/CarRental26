using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Service;
using CarRentalApi.Domain.Utils;
using Microsoft.Extensions.Logging;

namespace CarRentalApi.Domain.UseCases.Cars;

public sealed class CarUcSendToMaintenance(
   ICarRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<CarUcSendToMaintenance> _logger
) : ICarUcSendToMaintenance
{
   public async Task<Result> ExecuteAsync(Guid carId, CancellationToken ct)
   {
      _logger.LogInformation("CarUcSendToMaintenance start carId={id}", carId);

      var car = await _repository.FindByIdAsync(carId, ct);
      if (car is null)
      {
         _logger.LogWarning("CarUcSendToMaintenance rejected carId={id} errorCode={code}",
            carId, CarErrors.NotFound.Code);
         return Result.Failure(CarErrors.NotFound);
      }

      var result = car.SendToMaintenance();
      if (result.IsFailure)
      {
         _logger.LogWarning("CarUcSendToMaintenance rejected carId={id} errorCode={code}",
            carId, result.Error!.Code);
         return result;
      }

      await _unitOfWork.SaveAllChangesAsync("Car set to maintenance", ct);

      _logger.LogDebug("CarUcSendToMaintenance done carId={id}", carId);
      return Result.Success();
   }
}
