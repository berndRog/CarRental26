namespace CarRentalApi.Domain;

public interface IClock {
   DateTimeOffset UtcNow { get; }
}