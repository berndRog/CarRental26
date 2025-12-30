namespace CarRentalApi.Domain.Errors;

/// <summary>
/// Represents a business/domain error.
/// Comparable to a sealed error type in Kotlin.
/// </summary>
public sealed record DomainErrors(
   string Code,
   string Message
) {
   // Generic errors
   public static readonly DomainErrors None =
      new("none", string.Empty);

   public static readonly DomainErrors Unexpected =
      new("unexpected", "An unexpected domain error occurred.");
   
   public static readonly DomainErrors NotFound =
      new("domain.notFound", "Requested resource was not found."); 
   
   public static readonly DomainErrors Forbidden =
      new("domain.forbidden", "Operation is not allowed.");
   
   public static readonly DomainErrors InvalidGuidFormat =
      new("invalid_guid", "The provided identifier is not a valid GUID.");
}
