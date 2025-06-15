using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Domain.Interfaces;

public interface IValidator<in T>
{
    IEnumerable<ValidationResult> Validate (T? value);
}