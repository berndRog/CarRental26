using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApiTest.Domain.Entites;

public class CarUt {
   [Fact]
   public void Create_succeeds_with_valid_data() {
      // Act
      var result = Car.Create(
         category: CarCategory.Compact,
         manufacturer: "Volkswagen",
         model: "Golf",
         licensePlate: "BS-AB-123"
      );

      // Assert
      Assert.True(result.IsSuccess);
      Assert.NotNull(result.Value);
      Assert.Equal(CarStatus.Available, result.Value!.Status);
      Assert.Equal(CarCategory.Compact, result.Value.Category);
      Assert.Equal("Volkswagen", result.Value.Manufacturer);
      Assert.Equal("Golf", result.Value.Model);
      Assert.Equal("BS-AB-123", result.Value.LicensePlate);
   }
}
