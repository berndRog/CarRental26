using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain;

public interface ICarRepository {
   
   Task<int> CountCarsInCategoryAsync(
      CarCategory category,
      CancellationToken ct
   );
   
}