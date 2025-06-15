using System.ComponentModel.DataAnnotations;

using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Validators;

public sealed class CardExpiryDateValidator : IValidator<(int? Month, int? Year)>
{
    public const string MonthProperty = "expiry_month";
    public const string YearProperty = "expiry_year";

    public IEnumerable<ValidationResult> Validate((int? Month, int? Year) value)
    {
        var listOfResults = new List<ValidationResult>();
        if (value.Month is < 1 or > 12)
        {
            listOfResults.Add(new ValidationResult("Must be between 1-12", [MonthProperty]));
        }

        var currentYear = DateTimeOffset.Now.Year;
        var currentMonth = DateTimeOffset.Now.Month;
        if (value.Year <= currentYear)
        {
            if (value.Year == currentYear && value.Month <= currentMonth)
            {
                listOfResults.Add(new ValidationResult("Date must be in the future", [YearProperty, MonthProperty]));
            }
            else if (value.Year < currentYear)
            {
                listOfResults.Add(new ValidationResult("Year must be in the future", [YearProperty]));
            }
        }

        return listOfResults;
    }
}