using CarRentalApi.Domain.UseCases;
namespace BankingApi.Domain.UseCases.Owners;

public class ReservationUseCases(
   IReservationUcCreateDraft createCreateDraft,
   IReservationUcConfirm confirm,
   IReservationUcCancel cancel,
   IReservationUcExpire expire
): IReservationUseCases {
   public IReservationUcCreateDraft CreateCreateDraft { get; } = createCreateDraft;
   public IReservationUcConfirm Confirm { get; } = confirm;
   public IReservationUcCancel Cancel { get; } = cancel;
   public IReservationUcExpire Expire { get; } = expire;
}