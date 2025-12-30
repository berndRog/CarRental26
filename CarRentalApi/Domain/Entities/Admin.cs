using CarRentalApi.Domain.Enums;
using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.Entities;

public sealed class Admin : Employee {
   
   public AdminRights AdminRights { get; private set; } = AdminRights.ViewReports;

   // EF Core ctor
   private Admin() { }

   // Domain ctor
   private Admin(
      Guid id,
      string firstName,
      string lastName,
      string email,
      string personnelNumber,
      AdminRights adminRights,
      Address? address = null
   ) : base(id, firstName, lastName, email, personnelNumber, address) {
      AdminRights = adminRights;
   }

   // ---------- Factory (Result-based) ----------
   public static Result<Admin> Create(
      string firstName,
      string lastName,
      string email,
      string personnelNumber,
      string? id = null,
      AdminRights adminRights = AdminRights.ViewReports,
      Address? address = null
   ) {
      // Normalize input early
      firstName = firstName?.Trim() ?? string.Empty;
      lastName = lastName?.Trim() ?? string.Empty;
      email = email?.Trim() ?? string.Empty;
      personnelNumber = personnelNumber?.Trim() ?? string.Empty;
      
      var validation = ValidatePersonData(firstName, lastName, email);
      if (validation.IsFailure)
         return Result<Admin>.Failure(validation.Error);

      if (string.IsNullOrWhiteSpace(personnelNumber))
         return Result<Admin>.Failure(EmployeeErrors.PersonnelNumberIsRequired);
      
      var result = EntityId.Resolve(id, PersonErrors.InvalidId);
      if (result.IsFailure)
         return Result<Admin>.Failure(result.Error);
      var adminId = result.Value;
  
      var rightsValidation = ValidateAdminRights(adminRights);
      if (rightsValidation.IsFailure)
         return Result<Admin>.Failure(rightsValidation.Error);
      
      var admin = new Admin(
         adminId,
         firstName,
         lastName,
         email,
         personnelNumber,
         adminRights,
         address
      );

      return Result<Admin>.Success(admin);
   }

   public bool HasRight(AdminRights right) =>
      (AdminRights & right) == right;

   public Result Grant(AdminRights rights) {
      if (rights == AdminRights.None)
         return Result.Success();

      AdminRights |= rights;
      return Result.Success();
   }

   public Result Revoke(AdminRights rights) {
      if (rights == AdminRights.None)
         return Result.Success();

      AdminRights &= ~rights;

      // Optional: Regel, dass ein Admin mind. ViewReports behalten muss
      // -> wenn du das willst, kann ich es exakt als Invariante formulieren.

      return Result.Success();
   }

   public Result SetRights(AdminRights rights) {
      var validation = ValidateAdminRights(rights);
      if (validation.IsFailure)
         return validation;

      AdminRights = rights;
      return Result.Success();
   }

   private static Result ValidateAdminRights(AdminRights rights)
   {
      // Wenn du willst, dass ein Admin mindestens ViewReports haben muss:
      if (rights == AdminRights.None)
         return Result.Failure(AdminErrors.AdminRightsRequired);

      return Result.Success();
   }
}
