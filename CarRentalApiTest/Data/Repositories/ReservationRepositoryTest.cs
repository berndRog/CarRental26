using CarRentalApi.Data.Database;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Domain;
using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CarRentalApiTest.Data.Repositories;

public sealed class ReservationRepositoryTests : TestBase, IAsyncLifetime {
   private TestSeed _seed = null!;
   private SqliteConnection _dbConnection = null!;
   private CarRentalDbContext _dbContext = null!;
   private ReservationRepository _repository = null!;
   private UnitOfWork _unitOfWork = null!;

   // BEFORE each test (async)
   public async Task InitializeAsync() {

      var loggerRepository = CreateLogger<ReservationRepository>();
      var loggerUnitOfWork = CreateLogger<UnitOfWork>();

      _seed = new TestSeed();
      
      _dbConnection = new SqliteConnection("Filename=:memory:");
      await _dbConnection.OpenAsync();

      var options = new DbContextOptionsBuilder<CarRentalDbContext>()
         .UseSqlite(_dbConnection)
         .EnableSensitiveDataLogging()
         .Options;

      _dbContext = new CarRentalDbContext(options);
      await _dbContext.Database.EnsureCreatedAsync();

      _repository = new ReservationRepository(_dbContext, loggerRepository);
      _unitOfWork = new UnitOfWork(_dbContext, loggerUnitOfWork);

      //await Task.CompletedTask;
   }

   // AFTER each test (async)
   public async Task DisposeAsync() {
      if (_dbContext != null) {
         await _dbContext.DisposeAsync();
         _dbContext = null!;
      }

      if (_dbConnection != null) {
         await _dbConnection.CloseAsync();
         _dbConnection.Dispose();
         _dbConnection = null!;
      }

      //await Task.CompletedTask;
   }

   // =========
   // Helpers
   // =========
   // private static async Task<(SqliteConnection conn, CarRentalDbContext db, ReservationRepository repo)>
   //    CreateSutAsync() {
   //    
   //    var dbConnection = new SqliteConnection("Filename=:memory:");
   //    await dbConnection.OpenAsync();
   //
   //    var options = new DbContextOptionsBuilder<CarRentalDbContext>()
   //       .UseSqlite(dbConnection)
   //       .EnableSensitiveDataLogging()
   //       .Options;
   //    
   //    _dbContext = new CarRentalDbContext(options);
   //    await _dbContext.Database.EnsureCreatedAsync();
   //
   //    var repository = new ReservationRepository(dbContext, NullLogger<ReservationRepository>.Instance);
   //
   //    return (dbConnection, dbContext, repository);
   // }



   // =========
   // Tests
   // =========
   [Fact]
   public async Task FindByIdAsync_returns_reservation_when_found() {
      // Arrange
      var reservation1 = _seed.Reservation1;
      var id = reservation1.Id;

      _repository.Add(reservation1);
      await _unitOfWork.SaveAllChangesAsync();

      // Act
      var actual = await _repository.FindByIdAsync(id, CancellationToken.None);

      // Assert
      Assert.NotNull(actual);
      Assert.Equal(id, actual!.Id);
   }

   [Fact]
   public async Task FindByIdAsync_returns_null_when_not_found() {
      // Arrange
      var missingId = Guid.NewGuid();

      // Act
      var actual = await _repository.FindByIdAsync(missingId, CancellationToken.None);

      // Assert
      Assert.Null(actual);
   }
}


//    [Fact]
//    public async Task SelectDraftsToExpireAsync_returns_only_drafts_with_createdAt_lte_now() {
//
//          // Arrange
//          var now = DateTimeOffset.UtcNow;
//
//          // Should be included (Draft, CreatedAt <= now)
//          
//          var d1 = CreateReservation(Guid.NewGuid(), ReservationStatus.Draft, now.AddMinutes(-30));
//          var d2 = CreateReservation(Guid.NewGuid(), ReservationStatus.Draft, now); // boundary
//
//          // Should NOT be included (Draft, but CreatedAt > now)
//          var dFuture = CreateReservation(Guid.NewGuid(), ReservationStatus.Draft, now.AddMinutes(+1));
//
//          // Should NOT be included (not Draft)
//          var confirmed = CreateReservation(Guid.NewGuid(), ReservationStatus.Confirmed, now.AddMinutes(-60));
//          var cancelled = CreateReservation(Guid.NewGuid(), ReservationStatus.Cancelled, now.AddMinutes(-60));
//
//          db.Reservations.AddRange(d1, d2, dFuture, confirmed, cancelled);
//          await db.SaveChangesAsync();
//
//          // Act
//          var result = await repo.SelectDraftsToExpireAsync(now, CancellationToken.None);
//
//          // Assert
//          Assert.NotNull(result);
//
//          var ids = result.Select(r => r.Id).ToHashSet();
//          Assert.Contains(d1.Id, ids);
//          Assert.Contains(d2.Id, ids);
//
//          Assert.DoesNotContain(dFuture.Id, ids);
//          Assert.DoesNotContain(confirmed.Id, ids);
//          Assert.DoesNotContain(cancelled.Id, ids);
//       }
//    }
// }
//
// [Fact]
// public async Task Add_tracks_entity_as_Added_and_persists_after_SaveChanges() {
//    var (conn, db, repo) = await CreateSutAsync();
//    await using (conn)
//    await using (db) {
//       // Arrange
//       var id = Guid.NewGuid();
//       var r = CreateReservation(id, ReservationStatus.Draft, DateTimeOffset.UtcNow.AddMinutes(-5));
//
//       // Act
//       repo.Add(r);
//
//       // Assert (ChangeTracker)
//       var entry = db.Entry(r);
//       Assert.Equal(EntityState.Added, entry.State);
//
//       // Persist check
//       await db.SaveChangesAsync();
//       var reloaded = await db.Reservations.FirstOrDefaultAsync(x => x.Id == id);
//       Assert.NotNull(reloaded);
//    }
// }
//
// [Fact]
// public async Task Remove_tracks_entity_as_Deleted_and_deletes_after_SaveChanges() {
//    var (conn, db, repo) = await CreateSutAsync();
//    await using (conn)
//    await using (db) {
//       // Arrange
//       var id = Guid.NewGuid();
//       var r = CreateReservation(id, ReservationStatus.Draft, DateTimeOffset.UtcNow.AddMinutes(-5));
//
//       db.Reservations.Add(r);
//       await db.SaveChangesAsync();
//
//       // Act
//       repo.Remove(r);
//
//       // Assert (ChangeTracker)
//       var entry = db.Entry(r);
//       Assert.Equal(EntityState.Deleted, entry.State);
//
//       // Persist check
//       await db.SaveChangesAsync();
//       var reloaded = await db.Reservations.FirstOrDefaultAsync(x => x.Id == id);
//       Assert.Null(reloaded);
//    }
// }