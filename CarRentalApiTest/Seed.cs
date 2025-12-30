using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Utils;
using CarRentalApi.Domain.ValueObjects;
namespace CarRentalApiTest;

public sealed class TestSeed {
   public DateTimeOffset Now => DateTimeOffset.Parse("2026-01-01T00:00:00+00:00");

   // Readable IDs (keep them stable for teaching, debugging and tests)
   public string Reservation1Id = "10000000-0000-0000-0000-000000000000";
   public string Reservation2Id = "20000000-0000-0000-0000-000000000000";
   public string Reservation3Id = "30000000-0000-0000-0000-000000000000";

   public string Customer1Id = "00000000-1000-0000-0000-000000000000";
   public string Customer2Id = "00000000-2000-0000-0000-000000000000";

   public string Car1Id = "00000000-0000-0001-0000-000000000000";
   public string Car2Id = "00000000-0000-0002-0000-000000000000";
   public string Car3Id = "00000000-0000-0003-0000-000000000000";
   public string Car4Id = "00000000-0000-0004-0000-000000000000";
   public string Car5Id = "00000000-0000-0005-0000-000000000000";
   public string Car6Id = "00000000-0000-0006-0000-000000000000";
   public string Car7Id = "00000000-0000-0007-0000-000000000000";
   public string Car8Id = "00000000-0000-0008-0000-000000000000";
   public string Car9Id = "00000000-0000-0009-0000-000000000000";
   public string Car10Id = "00000000-0000-0010-0000-000000000000";
   public string Car11Id = "00000000-0000-0011-0000-000000000000";
   public string Car12Id = "00000000-0000-0012-0000-000000000000";
   public string Car13Id = "00000000-0000-0013-0000-000000000000";
   public string Car14Id = "00000000-0000-0014-0000-000000000000";
   public string Car15Id = "00000000-0000-0015-0000-000000000000";
   public string Car16Id = "00000000-0000-0016-0000-000000000000";
   public string Car17Id = "00000000-0000-0017-0000-000000000000";
   public string Car18Id = "00000000-0000-0018-0000-000000000000";
   public string Car19Id = "00000000-0000-0019-0000-000000000000";
   public string Car20Id = "00000000-0000-0020-0000-000000000000";

   public static Car Car1 { get; private set; } = null!;
   public static Car Car2 { get; private set; } = null!;
   public static Car Car3 { get; private set; } = null!;
   public static Car Car4 { get; private set; } = null!;
   public static Car Car5 { get; private set; } = null!;
   public static Car Car6 { get; private set; } = null!;
   public static Car Car7 { get; private set; } = null!;
   public static Car Car8 { get; private set; } = null!;
   public static Car Car9 { get; private set; } = null!;
   public static Car Car10 { get; private set; } = null!;
   public static Car Car11 { get; private set; } = null!;
   public static Car Car12 { get; private set; } = null!;
   public static Car Car13 { get; private set; } = null!;
   public static Car Car14 { get; private set; } = null!;
   public static Car Car15 { get; private set; } = null!;
   public static Car Car16 { get; private set; } = null!;
   public static Car Car17 { get; private set; } = null!;
   public static Car Car18 { get; private set; } = null!;
   public static Car Car19 { get; private set; } = null!;
   public static Car Car20 { get; private set; } = null!;

   // ---------- Convenience ----------
   public static IReadOnlyList<Car> Cars => [
      Car1, Car2, Car3, Car4, Car5, // Economy
      Car6, Car7, Car8, Car9, Car10, // Compact
      Car11, Car12, Car13, Car14, Car15, // Midsize
      Car16, Car17, Car18, Car19, Car20 // SUV
   ];

   
   public RentalPeriod Period1 => RentalPeriod.Create(
      DateTimeOffset.Parse("2030-05-01T10:00:00+00:00"),
      DateTimeOffset.Parse("2030-05-05T10:00:00+00:00")
   ).GetValueOrThrow();
   public RentalPeriod Period2 => RentalPeriod.Create(
      DateTimeOffset.Parse("2030-05-11T10:00:00+00:00"),
      DateTimeOffset.Parse("2030-05-15T10:00:00+00:00")
   ).GetValueOrThrow();
   
   public Reservation Reservation1 { get; private set; } = null!;
   public Reservation Reservation2 { get; private set;} = null!;
   public Reservation Reservation3 { get; private set; } = null!;


   public TestSeed() {
      
      Car1 = CreateCar(Car1Id, CarCategory.Economy, "VW", "Polo", "ECO-001");
      Car2 = CreateCar(Car2Id, CarCategory.Economy, "VW", "Polo", "ECO-002");
      Car3 = CreateCar(Car3Id, CarCategory.Economy, "VW", "Polo", "ECO-003");
      Car4 = CreateCar(Car4Id, CarCategory.Economy, "VW", "Polo", "ECO-004");
      Car5 = CreateCar(Car5Id, CarCategory.Economy, "VW", "Polo", "ECO-005");

      Car6 = CreateCar(Car6Id, CarCategory.Compact, "VW", "Golf", "COM-001");
      Car7 = CreateCar(Car7Id, CarCategory.Compact, "VW", "Golf", "COM-002");
      Car8 = CreateCar(Car8Id, CarCategory.Compact, "VW", "Golf", "COM-003");
      Car9 = CreateCar(Car9Id, CarCategory.Compact, "VW", "Golf", "COM-004");
      Car10 = CreateCar(Car10Id, CarCategory.Compact, "VW", "Golf", "COM-005");

      Car11 = CreateCar(Car11Id, CarCategory.Midsize, "BMW", "3 Series", "MID-001");
      Car12 = CreateCar(Car12Id, CarCategory.Midsize, "BMW", "3 Series", "MID-002");
      Car13 = CreateCar(Car13Id, CarCategory.Midsize, "BMW", "3 Series", "MID-003");
      Car14 = CreateCar(Car14Id, CarCategory.Midsize, "BMW", "3 Series", "MID-004");
      Car15 = CreateCar(Car14Id, CarCategory.Midsize, "BMW", "3 Series", "MID-005");

      Car16 = CreateCar(Car16Id, CarCategory.Suv, "Audi", "Q5", "SUV-001");
      Car17 = CreateCar(Car17Id, CarCategory.Suv, "Audi", "Q5", "SUV-002");
      Car18 = CreateCar(Car18Id, CarCategory.Suv, "Audi", "Q5", "SUV-003");
      Car19 = CreateCar(Car19Id, CarCategory.Suv, "Audi", "Q5", "SUV-004");
      Car20 = CreateCar(Car19Id, CarCategory.Suv, "Audi", "Q5", "SUV-005");


      Reservation1 = CreateReservation(
         id: Reservation1Id,
         customerId: Customer1Id,
         carCategory: CarCategory.Compact,
         period: Period1,
         createdAt: DateTimeOffset.Parse("2025-12-25T10:00:00+00:00")
      );
   }

   // ---------- Helper ----------
   private static Reservation CreateReservation(
      string id,
      string customerId,
      CarCategory carCategory,
      RentalPeriod period,
      DateTimeOffset createdAt
   ) {
      
      var result = Reservation.CreateDraft(
         customerId: customerId.ToGuid(),
         carCategory: carCategory,
         period: period,
         createdAt: createdAt,
         id: id
      );

      // Test seed must always be valid
      Assert.True(result.IsSuccess);
      return result.Value!;
   }
   
   private static Car CreateCar(
      string id,
      CarCategory category,
      string manufacturer,
      string model,
      string licensePlate
   ) {
      var result = Car.Create(
         category: category,
         manufacturer: manufacturer,
         model: model,
         licensePlate: licensePlate,
         id: id
      );

      // Test seed must always be valid
      Assert.True(result.IsSuccess);
      return result.Value!;
   }
}