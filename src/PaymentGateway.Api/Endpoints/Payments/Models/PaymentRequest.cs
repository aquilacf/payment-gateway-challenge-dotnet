using System.ComponentModel.DataAnnotations;

using PaymentGateway.Infrastructure.Validators;

namespace PaymentGateway.Api.Endpoints.Payments.Models;

public sealed class PaymentRequest : IValidatableObject
{
    public string? CardNumber { get; init; }
    public int? ExpiryMonth { get; init; }
    public int? ExpiryYear { get; init; }
    public string? Currency { get; init; }
    public int? Amount { get; init; }
    public string? Cvv { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var list = new List<ValidationResult>();

        var creditCardValidator = validationContext.GetService<CreditCardValidator>();
        if (creditCardValidator is not null)
        {
            list.AddRange(creditCardValidator.Validate(CardNumber));
        }

        var cvvValidator = validationContext.GetService<CvvValidator>();
        if (cvvValidator is not null)
        {
            list.AddRange(cvvValidator.Validate(Cvv));
        }

        var currencyValidator = validationContext.GetService<CurrencyValidator>();
        if (currencyValidator is not null)
        {
            list.AddRange(currencyValidator.Validate(Currency));
        }

        var cardExpiryDateValidatorValidator = validationContext.GetService<CardExpiryDateValidator>();
        if (cardExpiryDateValidatorValidator is not null)
        {
            list.AddRange(cardExpiryDateValidatorValidator.Validate((ExpiryMonth, ExpiryYear)));
        }

        if (Amount < 0)
        {
            list.AddRange(new ValidationResult("Must be positive", ["amount"]));
        }

        return list;
    }
}