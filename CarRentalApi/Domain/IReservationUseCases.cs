using CarRentalApi.Domain.UseCases.Reservations;
namespace CarRentalApi.Domain.UseCases;

public interface IReservationUseCases {
   IReservationUcCreateDraft CreateCreateDraft { get; }
   IReservationUcChangePeriod ChangePeriod { get; }
   IReservationUcConfirm Confirm { get; }
   IReservationUcCancel Cancel { get; }
   IReservationUcExpire Expire { get; }
}