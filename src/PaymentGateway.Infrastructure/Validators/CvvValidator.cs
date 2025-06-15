using System.ComponentModel.DataAnnotations;

using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Validators;

public sealed class CvvValidator : IValidator<string>
{
    public const string PropertyName = "cvv";

    public IEnumerable<ValidationResult> Validate(string? value)
    {
        var listOfResults = new List<ValidationResult>();
        if (string.IsNullOrWhiteSpace(value))
        {
            listOfResults.Add(new ValidationResult("Is required", [PropertyName]));
        }
        else
        {
            if (value.Length is < 3 or > 4)
            {
                listOfResults.Add(new ValidationResult("Must be 3-4 characters long", [PropertyName]));
            }

            if (!value.All(char.IsDigit))
            {
                listOfResults.Add(new ValidationResult("Must only contain digits", [PropertyName]));
            }
        }

        return listOfResults;
    }
}