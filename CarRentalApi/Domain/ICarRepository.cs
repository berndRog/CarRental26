using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain;

public interface ICarRepository {
   
   Task<Car?> FindByIdAsync(Guid id, CancellationToken ct);
   Task<bool> ExistsLicensePlateAsync(string licensePlate, CancellationToken ct);
   Task<int> CountCarsInCategoryAsync(CarCategory category, CancellationToken ct);
   
   Task<IReadOnlyList<Car>> SelectAsync(
      CarCategory? category,
      CarStatus? status,
      CancellationToken ct
   );
   
   void Add(Car car);
}