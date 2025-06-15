using System.ComponentModel.DataAnnotations;

using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Validators;

public sealed class CurrencyValidator : IValidator<string>
{
    public const string PropertyName = "currency";

    private readonly string[] _currencies = ["GBP", "BRL", "USD"];

    public IEnumerable<ValidationResult> Validate(string? value)
    {
        var listOfResults = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(value))
        {
            listOfResults.Add(new ValidationResult("Is required", [PropertyName]));
        }
        else
        {
            if (!_currencies.Contains(value.ToUpper()))
            {
                listOfResults.Add(new ValidationResult($"Must be one of: {string.Join(", ", _currencies)}",
                    [PropertyName]));
            }
        }

        return listOfResults;
    }
}