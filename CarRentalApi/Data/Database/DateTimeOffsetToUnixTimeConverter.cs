using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace CarRentalApi.Data.Database;

public sealed class DateTimeOffsetToIsoStringConverter() : ValueConverter<DateTimeOffset, string>(
      // ➜ C# → DB (write) - ISO 8601 String
      dto => dto.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
      // ➜ DB → C# (read) - Parse ISO String zurück
      iso => DateTimeOffset.Parse(iso, null, System.Globalization.DateTimeStyles.RoundtripKind)
) { }

/*
// epoch ist leider schlecht lesbar
public sealed class DateTimeOffsetToUnixTimeConverter() : ValueConverter<DateTimeOffset, long>(
      // ➜ C# → DB (write)
      dto => dto.ToUnixTimeMilliseconds(),
      // ➜ DB → C# (read)  ← THIS is the inverse direction
      millis => DateTimeOffset.FromUnixTimeMilliseconds(millis)
) { }
*/