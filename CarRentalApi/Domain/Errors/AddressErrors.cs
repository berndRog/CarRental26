namespace CarRentalApi.Domain.Errors;

public static class AddressErrors {
   public static readonly DomainErrors StreetIsRequired =
      new("address.street_required", "Street must not be empty.");

   public static readonly DomainErrors PostalCodeIsRequired =
      new("address.postal_code_required", "Postal code must not be empty.");

   public static readonly DomainErrors CityIsRequired =
      new("address.city_required", "City must not be empty.");

   public static readonly DomainErrors CountryIsRequired =
      new("address.country_required", "Country must not be empty.");
}