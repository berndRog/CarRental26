namespace CarRentalApi.Domain.UseCases;

public interface IReservationUseCases {
   IReservationUcCreateDraft CreateCreateDraft { get; }
   IReservationUcConfirm Confirm { get; }
   IReservationUcCancel Cancel { get; }
   IReservationUcExpire Expire { get; }
}