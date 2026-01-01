using CarRentalApi.Data.Database;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Domain;
using CarRentalApi.Domain.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
namespace CarRentalApiTest.Data.Repositories;

public sealed class ReservationRepositoryIt : TestBase, IAsyncLifetime {
   private TestSeed _seed = null!;
   private SqliteConnection _dbConnection = null!;
   private CarRentalDbContext _dbContext = null!;
   private ReservationRepository _repository = null!;
   private IUnitOfWork _unitOfWork = null!;

   public async Task InitializeAsync() {
      _seed = new TestSeed();

      _dbConnection = new SqliteConnection("Filename=:memory:");
      await _dbConnection.OpenAsync();

      var options = new DbContextOptionsBuilder<CarRentalDbContext>()
         .UseSqlite(_dbConnection)
         .EnableSensitiveDataLogging()
         .Options;

      _dbContext = new CarRentalDbContext(options);
      await _dbContext.Database.EnsureCreatedAsync();

      _repository = new ReservationRepository(_dbContext, CreateLogger<ReservationRepository>());
      _unitOfWork = new UnitOfWork(_dbContext, CreateLogger<UnitOfWork>());

   }

   public async Task DisposeAsync() {
      if (_dbContext != null) {
         await _dbContext.DisposeAsync();
         _dbContext = null!;
      }

      if (_dbConnection != null) {
         await _dbConnection.CloseAsync();
         await _dbConnection.DisposeAsync();
         _dbConnection = null!;
      }
   }

   [Fact]
   public async Task FindByIdAsync_returns_reservation_when_found() {
      // Arrange
      _dbContext.Reservations.AddRange(_seed.Reservations);
      await _unitOfWork.SaveAllChangesAsync();
      _dbContext.ChangeTracker.Clear(); // make assertions more realistic

      var id = Guid.Parse(_seed.Reservation1Id);

      // Act
      var actual = await _repository.FindByIdAsync(id, CancellationToken.None);

      // Assert
      Assert.NotNull(actual);
      Assert.Equal(id, actual!.Id);
   }

   [Fact]
   public async Task CountConfirmedOverlappingAsync_counts_only_confirmed_overlapping_and_ignores_given_id() {
      // Arrange
      var category = CarCategory.Compact;
      var start = _seed.Period1.Start;
      var end = _seed.Period1.End;

      // Persist seed reservations first (they are Draft initially)
      _dbContext.Reservations.AddRange(_seed.Reservations);
      await _unitOfWork.SaveAllChangesAsync();

      // Confirm 1..9 (10 stays Draft)
      var createdAt = DateTimeOffset.Parse("2025-12-25T10:00:00+00:00");
      var confirmAt = DateTimeOffset.Parse("2026-01-01T00:10:00+00:00");

      // 1..8 are overlapping, 9 is confirmed but non-overlapping, 10 stays draft
      foreach (var r in new[] {
                  _seed.Reservation1, _seed.Reservation2, _seed.Reservation3, _seed.Reservation4,
                  _seed.Reservation5, _seed.Reservation6, _seed.Reservation7, _seed.Reservation8,
                  _seed.Reservation9
               }) {
         var res = r.Confirm(confirmAt);
         Assert.True(res.IsSuccess);
      }

      await _dbContext.SaveChangesAsync();

      var ignoreId = Guid.Parse(_seed.Reservation1Id);

      // Act
      var count = await _repository.CountConfirmedOverlappingAsync(
         category: category,
         start: start,
         end: end,
         ignoreReservationId: ignoreId,
         ct: CancellationToken.None
      );

      // Assert
      // 8 overlapping confirmed - 1 ignored = 7
      Assert.Equal(7, count);
   }

   [Fact]
   public async Task SelectDraftsToExpireAsync_returns_only_drafts_created_before_or_at_now()
   {
      // Arrange
      _dbContext.Reservations.AddRange(_seed.Reservations);
      await _unitOfWork.SaveAllChangesAsync();

      // Make Reservation1..9 confirmed so they are not returned
      var confirmAt = DateTimeOffset.Parse("2026-01-01T00:10:00+00:00");
      foreach (var r in new[]
               { _seed.Reservation1, _seed.Reservation2, _seed.Reservation3, _seed.Reservation4,
                  _seed.Reservation5, _seed.Reservation6, _seed.Reservation7, _seed.Reservation8,
                  _seed.Reservation9 })
      {
         Assert.True(r.Confirm(confirmAt).IsSuccess);
      }
      await _unitOfWork.SaveAllChangesAsync("confirm for test", CancellationToken.None);
      _unitOfWork.ClearChangeTracker();

      var now = _seed.Now;

      // Act
      var drafts = await _repository.SelectDraftsToExpireAsync(now, CancellationToken.None);

      // Assert
      Assert.Single(drafts);
      Assert.Equal(Guid.Parse(_seed.Reservation10Id), drafts[0].Id);
      Assert.Equal(ReservationStatus.Draft, drafts[0].Status);
   }

}