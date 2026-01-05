using CarRentalApi.Domain.UseCases.Cars;
namespace CarRentalApi.Domain.UseCases;

public interface ICarUseCases {
   ICarUcCreate Create { get; }
   ICarUcSendToMaintenance SendToMaintenance { get; }
   ICarUcReturnFromMaintenance ReturnFromMaintenance { get; }
   ICarUcRetire Retire { get; }
}