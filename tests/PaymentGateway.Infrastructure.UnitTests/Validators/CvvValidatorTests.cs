using FluentAssertions;

using PaymentGateway.Infrastructure.Validators;

namespace PaymentGateway.Infrastructure.UnitTests.Validators;

public class CvvValidatorTests
{
    private readonly CvvValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12")]
    [InlineData("12345")]
    [InlineData("abc")]
    public void Validate_Invalid_ReturnsError(string? cvv)
    {
        var result = _validator.Validate(cvv).ToList();

        result.First().MemberNames.Should().Contain(CvvValidator.PropertyName);
    }

    [Fact]
    public void Validate_WhenValueIsTwoDigits_ReturnsLengthError()
    {
        var result = _validator.Validate("aaaaaa").ToList();

        result.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("000")]
    [InlineData("999")]
    [InlineData("0000")]
    [InlineData("9999")]
    public void Validate_WhenValueIsValidCvv_ReturnsNoErrors(string cvv)
    {
        var result = _validator.Validate(cvv);

        result.Should().BeEmpty();
    }
}