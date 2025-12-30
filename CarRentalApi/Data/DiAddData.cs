using CarRentalApi.Data.Database;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Domain;
using Microsoft.EntityFrameworkCore;
namespace CarRentalApi.Data.Extensions;

public static class DiAddDataExtensions {
   public static IServiceCollection AddData(
      this IServiceCollection services,
      IConfiguration configuration
   ) {
      
      services.AddDbContext<CarRentalDbContext>(options =>
         options.UseSqlite(
            configuration.GetConnectionString("BankingDb"))
      );

      // Unit of Work
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      // Repositories
      services.AddScoped<IReservationRepository, ReservationRepository>();

      return services;
   }
}