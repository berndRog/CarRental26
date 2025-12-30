using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain.UseCases;

public interface IReservationUcCreateDraft {
   Task<Result<Reservation>> ExecuteAsync(
      Guid customerId,
      CarCategory carCategory,
      DateTimeOffset start,
      DateTimeOffset end,
      string? id = null,
      CancellationToken ct = default!
   );
}