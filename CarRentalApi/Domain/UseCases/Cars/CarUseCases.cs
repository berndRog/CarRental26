using CarRentalApi.Domain.UseCases;
using CarRentalApi.Domain.UseCases.Cars;
namespace BankingApi.Domain.UseCases.Owners;

public class CarUseCases(
   ICarUcCreate create,
   ICarUcSendToMaintenance sendToMaintenance,
   ICarUcReturnFromMaintenance returnFromMaintenance,
   ICarUcRetire retire
): ICarUseCases {
   public ICarUcCreate Create { get; } = create;
   public ICarUcSendToMaintenance SendToMaintenance { get; } = sendToMaintenance;
   public ICarUcReturnFromMaintenance ReturnFromMaintenance { get; } = returnFromMaintenance;
   public ICarUcRetire Retire { get; } = retire;
}