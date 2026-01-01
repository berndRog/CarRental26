using CarRentalApi.Domain.Entities;
namespace CarRentalApi.Domain;

public interface IRentalRepository {
   // async Queries 
   Task<Rental?> FindByIdAsync(Guid id, CancellationToken ct);
   Task<Rental?> FindActiveByCarIdAsync(Guid carId, CancellationToken ct);
   Task<Rental?> FindActiveByReservationIdAsync(Guid reservationId, CancellationToken ct);

   // Commands are sync (track entity in DbContext)
   void Add(Rental rental);
}