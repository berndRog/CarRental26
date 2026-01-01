using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;
using Microsoft.Extensions.Logging;

namespace CarRentalApi.Domain.UseCases.Cars;

public sealed class CarUcReturnFromMaintenance(
   ICarRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<CarUcReturnFromMaintenance> _logger
) : ICarUcReturnFromMaintenance
{
   public async Task<Result> ExecuteAsync(Guid carId, CancellationToken ct)
   {
      _logger.LogInformation("CarUcReturnFromMaintenance start carId={id}", carId);

      var car = await _repository.FindByIdAsync(carId, ct);
      if (car is null)
      {
         _logger.LogWarning("CarUcReturnFromMaintenance rejected carId={id} errorCode={code}",
            carId, CarErrors.NotFound.Code);
         return Result.Failure(CarErrors.NotFound);
      }

      var result = car.ReturnFromMaintenance();
      if (result.IsFailure)
      {
         _logger.LogWarning("CarUcReturnFromMaintenance rejected carId={id} errorCode={code}",
            carId, result.Error!.Code);
         return result;
      }

      await _unitOfWork.SaveAllChangesAsync("Car returned from maintenance", ct);

      _logger.LogDebug("CarUcReturnFromMaintenance done carId={id}", carId);
      return Result.Success();
   }
}
