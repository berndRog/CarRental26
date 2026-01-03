using CarRentalApi.Domain.Errors;
using CarRentalApi.Domain.Utils;
namespace CarRentalApiTest.Domain.Entities;

public sealed class CustomerUt {
   // Creation via TestSeed
   [Fact]
   public void Customer_from_TestSeed_is_valid() {
      // Arrange
      var seed = new TestSeed();

      // Act
      var customer = seed.Customer1;

      // Assert
      Assert.NotNull(customer);
      Assert.Equal(seed.Customer1Id.ToGuid(), customer.Id);
      Assert.Equal("Erika", customer.FirstName);
      Assert.Equal("Mustermann", customer.LastName);
      Assert.Equal("e.mustermann@t-line.de", customer.Email);
   }

   /*
   [Fact]
   public void ChangeName_updates_first_and_last_name() {
      // Arrange
      var seed = new TestSeed();
      var customer = seed.Customer1;

      // Act
      var result = customer.ChangeName("Max", "Muster");

      // Assert
      Assert.True(result.IsSuccess);
      Assert.Equal("Max", customer.FirstName);
      Assert.Equal("Muster", customer.LastName);

      // Email must remain unchanged
      Assert.Equal("e.mustermann@t-line.de", customer.Email);
   }

   [Fact]
   public void ChangeName_rejects_empty_first_name() {
      var seed = new TestSeed();
      var result = seed.Customer1.ChangeName("", "Muster");

      Assert.True(result.IsFailure);
      Assert.Equal(PersonErrors.FirstNameIsRequired.Code, result.Error!.Code);
   }

   [Fact]
   public void ChangeName_rejects_empty_last_name() {
      var seed = new TestSeed();
      var result = seed.Customer1.ChangeName("Max", "");

      Assert.True(result.IsFailure);
      Assert.Equal(PersonErrors.LastNameIsRequired.Code, result.Error!.Code);
   }
   */
   
   [Fact]
   public void ChangeEmail_updates_email() {
      // Arrange
      var seed = new TestSeed();
      var customer = seed.Customer1;

      // Act
      var result = customer.ChangeEmail("new@mail.de");

      // Assert
      Assert.True(result.IsSuccess);
      Assert.Equal("new@mail.de", customer.Email);
   }

   [Fact]
   public void ChangeEmail_rejects_invalid_email() {
      // Arrange
      var seed = new TestSeed();
      var customer = seed.Customer1;

      // Act
      var result = customer.ChangeEmail("");

      // Assert
      Assert.True(result.IsFailure);
      Assert.Equal(PersonErrors.EmailIsRequired.Code, result.Error!.Code);
   }

   // ------------------------------------------------------------------
   // Entity equality (identity-based)
   // ------------------------------------------------------------------
   [Fact]
   public void Customers_with_same_Id_are_equal_even_if_properties_differ() {
      // Arrange
      var seed1 = new TestSeed();
      var seed2 = new TestSeed();

      var customerA = seed1.Customer1;
      var customerB = seed2.Customer1; // same Id, separate instance

      // Act + Assert
      Assert.NotSame(customerA, customerB); // different references
      Assert.True(customerA.Equals(customerB)); // same identity
      Assert.Equal(customerA.GetHashCode(), customerB.GetHashCode());
   }

   [Fact]
   public void Customers_with_different_Id_are_not_equal() {
      // Arrange
      var seed = new TestSeed();
      var customerA = seed.Customer1;
      var customerB = seed.Customer2;

      // Act + Assert
      Assert.False(customerA.Equals(customerB));
   }

   [Fact]
   public void Equality_operator_compares_identity_if_overloaded() {
      // Arrange
      var seed1 = new TestSeed();
      var seed2 = new TestSeed();

      var a = seed1.Customer1;
      var b = seed2.Customer1;

      // Assert
      Assert.True(a == b);
      Assert.False(a != b);
   }
}