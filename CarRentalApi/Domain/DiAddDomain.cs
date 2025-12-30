using BankingApi.Domain.UseCases.Owners;
using CarRentalApi.Domain.Utils;
using CarRentalApi.Domain.UseCases;
using CarRentalApi.Domain.UseCases.Reservations;
namespace CarRentalApi.Domain.Extensions;

public static class DiAddDomainExtensions {
   
   public static IServiceCollection AddDomain(
      this IServiceCollection services
   ) {
      
      // ----------------------------
      // Reservation UseCases
      // ----------------------------
      services.AddScoped<IReservationUcCreateDraft, ReservationUcCreateDraft>();
      services.AddScoped<IReservationUcConfirm, ReservationUcConfirm>();      
      services.AddScoped<IReservationUcCancel, ReservationUcCancel>();
      services.AddScoped<IReservationUcExpire, ReservationUcExpire>();
      services.AddScoped<IReservationUseCases, ReservationUseCases>();

      return services;
   }
}