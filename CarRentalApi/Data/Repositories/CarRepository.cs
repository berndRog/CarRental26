using CarRentalApi.Data.Database;
using CarRentalApi.Domain;
using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.Data.Repositories;

public sealed class CarRepository(
   CarRentalDbContext _dbContext,
   ILogger<CarRepository> _logger
) : ICarRepository {
   public async Task<Car?> FindByIdAsync(Guid id, CancellationToken ct) {
      _logger.LogDebug("Load Car by Id ({Id})", id.To8());
      return await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == id, ct);
   }

   public async Task<bool> ExistsLicensePlateAsync(
      string licensePlate, 
      CancellationToken ct
   ) {
      var normalized  = licensePlate.Trim().ToUpperInvariant();

      _logger.LogDebug("Check license plate exists ({Plate})", normalized);
      return await _dbContext.Cars
         .AnyAsync(x => x.LicensePlate == normalized, ct);
   }

   public async Task<int> CountCarsInCategoryAsync(
      CarCategory category,
      CancellationToken ct
   ) {
      _logger.LogDebug(
         "Count cars in category={CarCategory}",
         category
      );

      var count = await _dbContext.Cars
         .Where(c => c.CarCategory == category)
         .CountAsync(ct);

      _logger.LogDebug(
         "Found {Count} cars in category={CarCategory}",
         count, category
      );

      return count;
   }

   public async Task<IReadOnlyList<Car>> SelectAsync(
      CarCategory? category,
      CarStatus? status,
      CancellationToken ct
   ) {
      _logger.LogDebug("Select Cars category={Cat} status={CarStatus}", category, status);

      var query = _dbContext.Cars.AsQueryable();

      if (category is not null)
         query = query.Where(x => x.CarCategory == category.Value);

      if (status is not null)
         query = query.Where(x => x.CarStatus == status.Value);

      return await query
         .OrderBy(x => x.CarCategory)
         .ThenBy(x => x.LicensePlate)
         .ToListAsync(ct);
   }

   public void Add(Car car) {
      _logger.LogDebug("Add Car ({Id})", car.Id.To8());
      _dbContext.Cars.Add(car);
   }
}