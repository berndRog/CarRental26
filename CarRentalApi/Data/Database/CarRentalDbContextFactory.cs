using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarRentalApi.Data.Database;

public class CarRentalDbContextFactory : IDesignTimeDbContextFactory<CarRentalDbContext>
{
   public CarRentalDbContext CreateDbContext(string[] args)
   {
      
      var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false)
         .AddJsonFile("appsettings.Development.json", optional: true)
         .Build();
      var connectionString = configuration.GetConnectionString("BankingDb");
      
      var optionsBuilder = new DbContextOptionsBuilder<CarRentalDbContext>();
        
      // Passen Sie den Connection String an Ihre Umgebung an
      optionsBuilder.UseSqlite(connectionString);
      // Oder für SQL Server:
      // optionsBuilder.UseSqlServer("Server=localhost;Database=banking_dev;Trusted_Connection=True;");
        
      return new CarRentalDbContext(optionsBuilder.Options);
   }
}
