using FluentAssertions;

using PaymentGateway.Infrastructure.Validators;

namespace PaymentGateway.Infrastructure.UnitTests.Validators;

public sealed class CurrencyValidatorTests
{
    private readonly CurrencyValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("xxx")]
    public void Validate_Invalid_ReturnsError(string? creditCard)
    {
        var result = _validator.Validate(creditCard).ToList();

        result.First().MemberNames.Should().Contain(CurrencyValidator.PropertyName);
    }

    [Theory]
    [InlineData("usd")]
    [InlineData("bRl")]
    [InlineData("GBP")]
    public void Validate_WhenValueIsValidCreditCard_ReturnsNoErrors(string currency)
    {
        var result = _validator.Validate(currency);

        result.Should().BeEmpty();
    }
}