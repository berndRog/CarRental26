namespace CarRentalApi.Domain.Errors;

public static class EmployeeErrors {
   
   public static readonly DomainErrors PersonnelNumberIsRequired =
      new("employee.personnel_number_required", "Personnel number must not be empty.");

   public static readonly DomainErrors PersonnelNumberInvalidFormat =
      new("employee.personnel_number_invalid_format", "Personnel number has an invalid format.");

}
