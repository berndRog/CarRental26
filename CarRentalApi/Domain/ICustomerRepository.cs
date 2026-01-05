using CarRentalApi.Domain.Entities;
using CarRentalApi.Domain.Enums;
namespace CarRentalApi.Domain;

public interface ICustomerRepository {
   
   Task<Customer?> FindByIdAsync(Guid id, CancellationToken ct);
   
   Task<IReadOnlyList<Customer>> SelectAsync(
      CancellationToken ct
   );
   
   void Add(Customer customer);
}