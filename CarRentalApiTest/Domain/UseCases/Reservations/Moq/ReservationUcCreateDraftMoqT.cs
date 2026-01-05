using CarRentalApi.Domain;
using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.UseCases.Reservations;
using CarRentalApi.Domain.Utils;
using CarRentalApiTest.Domain.Utils;
using Microsoft.Extensions.Logging;
using Moq;
namespace CarRentalApiTest.Domain.UseCases.Reservations;

public sealed class ReservationUcCreateDraftUt {
   private readonly Mock<IReservationRepository> _repo = new(MockBehavior.Strict);
   private readonly Mock<IUnitOfWork> _uow = new(MockBehavior.Strict);
   private readonly Mock<ILogger<ReservationUcCreateDraft>> _logger = new();

   [Fact]
   public async Task ExecuteAsync_returns_StartDateInPast_when_start_is_not_in_future() {
      // Arrange
      var seed = new TestSeed();
      var clock = new FakeClock(seed.Now);

      var sut = new ReservationUcCreateDraft(
         _repository: _repo.Object,
         _unitOfWork: _uow.Object,
         _logger: _logger.Object,
         _clock: clock
      );

      var start = clock.UtcNow; // not allowed: must be > now
      var end = clock.UtcNow.AddHours(1);

      // Act
      var result = await sut.ExecuteAsync(
         customerId: seed.Customer1Id.ToGuid(),
         carCategory: CarCategory.Compact,
         start: start,
         end: end,
         id: seed.Reservation1Id,
         ct: CancellationToken.None
      );

      // Assert
      Assert.True(result.IsFailure);
      Assert.Equal(ReservationErrors.StartDateInPast.Code, result.Error.Code);

      _repo.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
      _uow.Verify(u => u.SaveAllChangesAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);

      _repo.VerifyAll();
      _uow.VerifyAll();
   }

   [Fact]
   public async Task ExecuteAsync_returns_InvalidPeriod_when_end_is_not_after_start() {
      // Arrange
      var seed = new TestSeed();
      var clock = new FakeClock(seed.Now);

      var sut = new ReservationUcCreateDraft(
         _repository: _repo.Object,
         _unitOfWork: _uow.Object,
         _logger: _logger.Object,
         _clock: clock
      );

      var start = clock.UtcNow.AddDays(1);
      var end = start; // invalid period

      // Act
      var result = await sut.ExecuteAsync(
         customerId: seed.Customer1Id.ToGuid(),
         carCategory: CarCategory.Compact,
         start: start,
         end: end,
         id: seed.Reservation1Id,
         ct: CancellationToken.None
      );

      // Assert
      Assert.True(result.IsFailure);
      Assert.Equal(ReservationErrors.InvalidPeriod.Code, result.Error.Code);

      _repo.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
      _uow.Verify(u => u.SaveAllChangesAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);

      _repo.VerifyAll();
      _uow.VerifyAll();
   }

   [Fact]
   public async Task ExecuteAsync_success_adds_reservation_and_saves() {
      // Arrange
      var seed = new TestSeed();
      var clock = new FakeClock(seed.Now);

      _repo.Setup(r => r.Add(It.IsAny<Reservation>()));

      // Your IUnitOfWork returns Task<bool> in your project
      _uow.Setup(u => u.SaveAllChangesAsync("Reservation draft added", It.IsAny<CancellationToken>()))
         .ReturnsAsync(true);

      var sut = new ReservationUcCreateDraft(
         _repository: _repo.Object,
         _unitOfWork: _uow.Object,
         _logger: _logger.Object,
         _clock: clock
      );

      var customerId = seed.Customer1Id.ToGuid();
      var start = clock.UtcNow.AddDays(10);
      var end = clock.UtcNow.AddDays(12);
      var id = seed.Reservation2Id;

      // Act
      var result = await sut.ExecuteAsync(
         customerId: customerId,
         carCategory: CarCategory.Compact,
         start: start,
         end: end,
         id: id,
         ct: CancellationToken.None
      );

      // Assert
      Assert.True(result.IsSuccess);

      var reservation = result.Value;
      Assert.Equal(Guid.Parse(id), reservation.Id);
      Assert.Equal(customerId, reservation.CustomerId);
      Assert.Equal(CarCategory.Compact, reservation.CarCategory);
      Assert.Equal(ReservationStatus.Draft, reservation.ResStatus);

      Assert.Equal(start, reservation.Period.Start);
      Assert.Equal(end, reservation.Period.End);

      // createdAt must be the _clock's "now"
      Assert.Equal(clock.UtcNow, reservation.CreatedAt);

      _repo.Verify(r => r.Add(It.Is<Reservation>(x => x.Id == Guid.Parse(id))), Times.Once);
      _uow.Verify(u => u.SaveAllChangesAsync("Reservation draft added", It.IsAny<CancellationToken>()), Times.Once);

      _repo.VerifyAll();
      _uow.VerifyAll();
   }
}