using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace CarRentalApi.Data.Database;

public sealed class DateTimeOffsetToUnixTimeConverter() : ValueConverter<DateTimeOffset, long>(
      // ➜ C# → DB (write)
      dto => dto.ToUnixTimeMilliseconds(),
      // ➜ DB → C# (read)  ← THIS is the inverse direction
      millis => DateTimeOffset.FromUnixTimeMilliseconds(millis)
) { }