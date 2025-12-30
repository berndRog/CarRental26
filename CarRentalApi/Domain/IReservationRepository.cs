using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain;

public interface IReservationRepository {
   
   // Queries are async
   Task<Reservation?> FindByIdAsync(Guid id, CancellationToken ct);
   
   Task<int> CountConfirmedOverlappingAsync(
      CarCategory category,
      DateTimeOffset start,
      DateTimeOffset end,
      Guid ignoreReservationId,
      CancellationToken ct
   );
   
   Task<IReadOnlyList<Reservation>> SelectDraftsToExpireAsync(
      DateTimeOffset now, 
      CancellationToken ct
   );
   
   // Commands are sync, they just change the in-memory repository state
   void Add(Reservation reservation);
}


