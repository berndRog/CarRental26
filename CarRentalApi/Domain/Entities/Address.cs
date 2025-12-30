using CarRentalApi.Domain.Errors;
namespace CarRentalApi.Domain.Entities;

// Address is an owned value object without identity.
// It is immutable and fully replaced on change.
public sealed record class Address(
   string Street,
   string PostalCode,
   string City,
   string Country
) {
   public static Result<Address> Create(
      string street,
      string postalCode,
      string city,
      string country
   ) {
      if (string.IsNullOrWhiteSpace(street))
         return Result<Address>.Failure(AddressErrors.StreetIsRequired);

      if (string.IsNullOrWhiteSpace(postalCode))
         return Result<Address>.Failure(AddressErrors.PostalCodeIsRequired);

      if (string.IsNullOrWhiteSpace(city))
         return Result<Address>.Failure(AddressErrors.CityIsRequired);

      if (string.IsNullOrWhiteSpace(country))
         return Result<Address>.Failure(AddressErrors.CountryIsRequired);

      return Result<Address>.Success(
         new Address(
            street.Trim(),
            postalCode.Trim(),
            city.Trim(),
            country.Trim()
         )
      );
   }
}
