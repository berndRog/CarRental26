using CarRentalApi.Domain.UseCases;
using CarRentalApi.Domain.UseCases.Reservations;
namespace BankingApi.Domain.UseCases.Owners;

public class ReservationUseCases(
   IReservationUcCreateDraft createCreateDraft,
   IReservationUcChangePeriod changePeriod,
   IReservationUcConfirm confirm,
   IReservationUcCancel cancel,
   IReservationUcExpire expire
): IReservationUseCases {
   public IReservationUcCreateDraft CreateCreateDraft { get; } = createCreateDraft;
   public IReservationUcChangePeriod ChangePeriod { get; } = changePeriod;
   public IReservationUcConfirm Confirm { get; } = confirm;
   public IReservationUcCancel Cancel { get; } = cancel;
   public IReservationUcExpire Expire { get; } = expire;
}