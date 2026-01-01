using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.UseCases.Cars;

public interface ICarUcSendToMaintenance {
   Task<Result> ExecuteAsync(Guid carId, CancellationToken ct);
}
