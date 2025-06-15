using System.ComponentModel.DataAnnotations;

using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Validators;

public sealed class CreditCardValidator : IValidator<string>
{
    public const string PropertyName = "card_number";

    public IEnumerable<ValidationResult> Validate(string? value)
    {
        var listOfResults = new List<ValidationResult>();
        if (string.IsNullOrWhiteSpace(value))
        {
            listOfResults.Add(
                new ValidationResult("Is required", [PropertyName]));
        }
        else
        {
            if (value.Length is < 14 or > 19)
            {
                listOfResults.Add(
                    new ValidationResult("Must be between 14-19 characters", [PropertyName]));
            }

            if (!value.All(char.IsDigit))
            {
                listOfResults.Add(
                    new ValidationResult("Must only contain digits", [PropertyName]));
            }
        }

        return listOfResults;
    }
}