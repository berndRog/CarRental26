using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.UseCases.Cars;

public interface ICarUcRetire {
   Task<Result> ExecuteAsync(Guid carId, CancellationToken ct);
}