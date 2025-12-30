namespace CarRentalApi.Domain.Errors;

public static class CustomerErrors {

   public static readonly DomainErrors NotFound =
      new("customer.not_found", "Customer not found.");
}
