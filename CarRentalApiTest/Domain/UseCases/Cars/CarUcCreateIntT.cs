
using CarRentalApi.Data.Database;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.UseCases.Fleet;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarRentalApiTest.Domain.UseCases.Fleet;

public sealed class CarUcCreateIt : TestBase, IAsyncLifetime
{
   private SqliteConnection _dbConnection = null!;
   private CarRentalDbContext _dbContext = null!;
   private CarRepository _repo = null!;
   private UnitOfWork _uow = null!;
   private CarUcCreate _sut = null!;

   public async Task InitializeAsync()
   {
      _dbConnection = new SqliteConnection("Filename=:memory:");
      await _dbConnection.OpenAsync();

      var options = new DbContextOptionsBuilder<CarRentalDbContext>()
         .UseSqlite(_dbConnection)
         .EnableSensitiveDataLogging()
         .Options;

      _dbContext = new CarRentalDbContext(options);
      await _dbContext.Database.EnsureCreatedAsync();

      _repo = new CarRepository(_dbContext, CreateLogger<CarRepository>());
      _uow  = new UnitOfWork(_dbContext, CreateLogger<UnitOfWork>());

      // Seed one car to test uniqueness
      var seedCar = CarRentalApi.Domain.Entities.Car.Create(
         CarCategory.Economy, "VW", "Polo", "ECO-001", "00090000-0000-0000-0000-000000000000"
      ).Value!;
      _repo.Add(seedCar);
      await _uow.SaveAllChangesAsync("Seed car", CancellationToken.None);

      _sut = new CarUcCreate(_repo, _uow, CreateLogger<CarUcCreate>());
   }

   public async Task DisposeAsync()
   {
      if (_dbContext != null)
      {
         await _dbContext.DisposeAsync();
         _dbContext = null!;
      }

      if (_dbConnection != null)
      {
         await _dbConnection.CloseAsync();
         await _dbConnection.DisposeAsync();
         _dbConnection = null!;
      }
   }

   [Fact]
   public async Task ExecuteAsync_returns_unique_error_when_license_plate_exists()
   {
      var result = await _sut.ExecuteAsync(
         CarCategory.Economy, "VW", "Polo", "ECO-001",
         id: "00100000-0000-0000-0000-000000000000",
         ct: CancellationToken.None
      );

      Assert.True(result.IsFailure);
      Assert.Equal(CarErrors.LicensePlateMustBeUnique.Code, result.Error.Code);
   }

   [Fact]
   public async Task ExecuteAsync_success_persists_car()
   {
      var result = await _sut.ExecuteAsync(
         CarCategory.Compact, "VW", "Golf", "COM-999",
         id: "00110000-0000-0000-0000-000000000000",
         ct: CancellationToken.None
      );

      Assert.True(result.IsSuccess);

      var fromDb = await _repo.FindByIdAsync(Guid.Parse("00110000-0000-0000-0000-000000000000"), CancellationToken.None);
      Assert.NotNull(fromDb);
      Assert.Equal("COM-999", fromDb!.LicensePlate);
      Assert.Equal(CarStatus.Available, fromDb.Status);
   }
}
