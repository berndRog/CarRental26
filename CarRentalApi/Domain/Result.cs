using CarRentalApi.Domain.Errors;
namespace CarRentalApi.Domain;

// Nicht-generisches Result für Operationen ohne Rückgabewert
public readonly struct Result {
   
   public bool IsSuccess { get; }
   public bool IsFailure => !IsSuccess;
   public DomainErrors? Error { get; }

   private Result(bool isSuccess, DomainErrors? error = null) {
      IsSuccess = isSuccess;
      Error = error;
   }

   public static Result Success() => new(true);
   public static Result Failure(DomainErrors error) => new(false, error);
}

// Generisches Result<T>:  Echte binäres Zustände
public readonly struct Result<T> {
   
   public bool IsSuccess { get; }
   public bool IsFailure => !IsSuccess;
   public T? Value { get; }
   public DomainErrors? Error { get; }

   private Result(T value) {
      IsSuccess = true;
      Value = value;
      Error = null;
   }

   private Result(DomainErrors error) {
      IsSuccess = false;
      Value = default;
      Error = error;
   }

   public static Result<T> Success(T value) => new(value);
   public static Result<T> Failure(DomainErrors errors) => new(errors);
   
   public T GetValueOrDefault(T defaultValue = default!) {
      return IsSuccess && Value is not null ? Value : defaultValue;
   }

   public T GetValueOrThrow() {
      if (!IsSuccess || Value is null)
         throw new InvalidOperationException($"Result failed: {Error}");
      return Value;
   }
   
   public Result<T> OnSuccess(Action<T> action) {
      if (IsSuccess && Value is not null)
         action(Value);
      return this;
   }

   public Result<T> OnFailure(Action<DomainErrors> action) {
      if (!IsSuccess && Error is not null)
         action(Error);
      return this;
   }
   
   public TResult Fold<TResult>(
      Func<T, TResult> onSuccess,
      Func<DomainErrors, TResult> onFailure
   ) {
      return IsSuccess && Value is not null
         ? onSuccess(Value)
         : onFailure(Error!);
   }
   /*
   public Result<TResult> Map<TResult>(Func<T, TResult> mapper) {
      return IsSuccess && Value is not null
         ? Result<TResult>.Success(mapper(Value))
         : Result<TResult>.Failure(Error!);
   }
   
   public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> binder) {
      return IsSuccess && Value is not null
         ? binder(Value)
         : Result<TResult>.Failure(Error!);
   }
   */
}