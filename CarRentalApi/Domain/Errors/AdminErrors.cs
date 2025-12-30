namespace CarRentalApi.Domain.Errors;

public static class AdminErrors {
   public static readonly DomainErrors AdminRightsRequired =
      new("admin.rights_required", "Admin must have at least one admin right.");
}
