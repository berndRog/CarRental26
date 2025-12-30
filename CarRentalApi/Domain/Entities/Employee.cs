using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;
namespace CarRentalApi.Domain.Entities;

public class Employee: Person {

   public string PersonnelNumber { get; protected set; } = string.Empty;

   // EF Core ctor
   protected Employee() { }

   // Domain ctor
   protected Employee(
      Guid id,
      string firstName,
      string lastName,
      string email,
      string personnelNumber,
      Address? address
   ) : base(id, firstName, lastName, email, address) {
      PersonnelNumber = personnelNumber;
   }

   // ---------- Factory (Result-based) ----------
   public static Result<Employee> Create(
      string firstName,
      string lastName,
      string email,
      string personnelNumber,
      string? id = null,
      Address? address = null
   ) {
      // Normalize input early
      firstName = firstName?.Trim() ?? string.Empty;
      lastName = lastName?.Trim() ?? string.Empty;
      email = email?.Trim() ?? string.Empty;
      personnelNumber = personnelNumber?.Trim() ?? string.Empty;
      
      var baseValidation = ValidatePersonData(firstName, lastName, email);
      if (baseValidation.IsFailure)
         return Result<Employee>.Failure(baseValidation.Error);

      if (string.IsNullOrWhiteSpace(personnelNumber))
         return Result<Employee>.Failure(EmployeeErrors.PersonnelNumberIsRequired);

      var result = EntityId.Resolve(id, PersonErrors.InvalidId);
      if (result.IsFailure)
         return Result<Employee>.Failure(result.Error);
      var employeeId = result.Value;
      
      var employee = new Employee(
         employeeId,
         firstName,
         lastName,
         email,
         personnelNumber,
         address
      );

      return Result<Employee>.Success(employee);
   }
   
}