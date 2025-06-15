using FluentAssertions;

using PaymentGateway.Infrastructure.Validators;

namespace PaymentGateway.Infrastructure.UnitTests.Validators;

public sealed class CreditCardValidatorTests
{
    private readonly CreditCardValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("              ")]
    [InlineData("12345")]
    [InlineData("12345678900000000000")]
    [InlineData("aaaaaaaaaaaaaa")]
    public void Validate_Invalid_ReturnsError(string? creditCard)
    {
        var result = _validator.Validate(creditCard).ToList();

        result.First().MemberNames.Should().Contain(CreditCardValidator.PropertyName);
    }

    [Fact]
    public void Validate_WhenValueIsFewerDigits_ReturnsLengthError()
    {
        var result = _validator.Validate("aaaaaaaa").ToList();

        result.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("12345678900000")]
    [InlineData("1234567890000000000")]
    public void Validate_WhenValueIsValidCreditCard_ReturnsNoErrors(string creditCard)
    {
        var result = _validator.Validate(creditCard);

        result.Should().BeEmpty();
    }
}