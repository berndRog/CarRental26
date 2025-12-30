namespace CarRentalApi.Domain.Errors;

public static class PersonErrors {
   public static readonly DomainErrors InvalidId =
      new("person.invalid_id", "The provided id is invalid.");

   public static readonly DomainErrors FirstNameIsRequired =
      new("person.first_name_required", "First name must not be empty.");

   public static readonly DomainErrors LastNameIsRequired =
      new("person.last_name_required", "Last name must not be empty.");

   public static readonly DomainErrors EmailIsRequired =
      new("person.email_required", "Email must not be empty.");

   public static readonly DomainErrors EmailInvalidFormat =
      new("person.email_invalid_format", "Email has an invalid format.");
}
