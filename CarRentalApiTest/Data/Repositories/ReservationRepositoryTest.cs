using CarRentalApi.Data.Database;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Domain.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
namespace CarRentalApiTest.Data.Repositories;

public sealed class ReservationRepositoryIt : TestBase, IAsyncLifetime {
   private TestSeed _seed = null!;
   private SqliteConnection _dbConnection = null!;
   private CarRentalDbContext _dbContext = null!;
   private ReservationRepository _repo = null!;
   private UnitOfWork _uow = null!;

   // BEFORE each test (async)
   public async Task InitializeAsync() {
      _seed = new TestSeed();

      _dbConnection = new SqliteConnection("Filename=:memory:");
      await _dbConnection.OpenAsync();

      var options = new DbContextOptionsBuilder<CarRentalDbContext>()
         .UseSqlite(_dbConnection)
         .EnableSensitiveDataLogging()
         .Options;

      _dbContext = new CarRentalDbContext(options);

      // Ensure schema exists (in-memory SQLite is empty per connection)
      await _dbContext.Database.EnsureCreatedAsync();

      _repo = new ReservationRepository(_dbContext, CreateLogger<ReservationRepository>());
      _uow = new UnitOfWork(_dbContext, CreateLogger<UnitOfWork>());

      // Seed reservations (tracked by EF Core)
      foreach (var r in _seed.AllReservationsForRepoTests)
         _repo.Add(r);

      await _uow.SaveAllChangesAsync("Seed reservations", CancellationToken.None);
   }

   // AFTER each test (async)
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
      var id = Guid.Parse(_seed.Reservation1Id);

      // Act
      var actual = await _repo.FindByIdAsync(id, CancellationToken.None);

      // Assert
      Assert.NotNull(actual);
      Assert.Equal(id, actual!.Id);
   }

   [Fact]
   public async Task FindByIdAsync_returns_null_when_not_found() {
      // Act
      var actual = await _repo.FindByIdAsync(Guid.NewGuid(), CancellationToken.None);

      // Assert
      Assert.Null(actual);
   }

   [Fact]
   public async Task CountConfirmedOverlappingAsync_counts_only_confirmed_overlapping_and_excludes_ignore_id() {
      // Arrange
      // Seed contains 8 confirmed overlapping reservations (Compact).
      // Ignore Reservation3 -> expect 7.
      var start = _seed.Period1.Start.AddHours(1);
      var end = _seed.Period1.End.AddHours(-1);
      var ignoreId = Guid.Parse(_seed.Reservation3Id);

      // Act
      var count = await _repo.CountConfirmedOverlappingAsync(
         category: CarCategory.Compact,
         start: start,
         end: end,
         ignoreReservationId: ignoreId,
         ct: CancellationToken.None
      );

      // Assert
      Assert.Equal(7, count);
   }

   [Fact]
   public async Task CountConfirmedOverlappingAsync_returns_1_for_ok_non_overlapping_period() {
      // Arrange
      // Seed contains exactly one confirmed reservation in PeriodOkNonOverlapping (Reservation9).
      var start = _seed.PeriodOkNonOverlapping.Start;
      var end = _seed.PeriodOkNonOverlapping.End;

      // Act
      var count = await _repo.CountConfirmedOverlappingAsync(
         category: CarCategory.Compact,
         start: start,
         end: end,
         ignoreReservationId: Guid.Empty,
         ct: CancellationToken.None
      );

      // Assert
      Assert.Equal(1, count);
   }

   [Fact]
   public async Task CountConfirmedOverlappingAsync_returns_0_when_category_differs() {
      // Arrange
      var start = _seed.Period1.Start.AddHours(1);
      var end = _seed.Period1.End.AddHours(-1);

      // Act
      var count = await _repo.CountConfirmedOverlappingAsync(
         category: CarCategory.Suv, // different category than seed reservations (Compact)
         start: start,
         end: end,
         ignoreReservationId: Guid.Empty,
         ct: CancellationToken.None
      );

      // Assert
      Assert.Equal(0, count);
   }

   [Fact]
   public async Task SelectDraftsToExpireAsync_returns_only_drafts_created_before_or_at_now() {
      // Arrange
      var now = _seed.Now;

      // Act
      var drafts = await _repo.SelectDraftsToExpireAsync(now, CancellationToken.None);

      // Assert
      Assert.NotNull(drafts);
      Assert.True(drafts.Count >= 1);

      // Seed has exactly one old draft (Reservation10)
      var expectedId = Guid.Parse(_seed.Reservation10Id);
      Assert.Contains(drafts, r => r.Id == expectedId);

      Assert.All(drafts, r => {
         Assert.Equal(ReservationStatus.Draft, r.Status);
         Assert.True(r.CreatedAt <= now);
      });
   }

   [Fact]
   public async Task Remove_removes_reservation_from_database() {
      // Arrange
      var id = Guid.Parse(_seed.Reservation9Id);
      var existing = await _repo.FindByIdAsync(id, CancellationToken.None);
      Assert.NotNull(existing);

      // Act
      _repo.Remove(existing!);
      await _uow.SaveAllChangesAsync("Remove reservation", CancellationToken.None);

      // Assert
      var after = await _repo.FindByIdAsync(id, CancellationToken.None);
      Assert.Null(after);
   }
}