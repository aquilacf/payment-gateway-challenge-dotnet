using FluentAssertions;

using PaymentGateway.Infrastructure.Validators;

namespace PaymentGateway.Infrastructure.UnitTests.Validators;

public class CardExpiryDateValidatorTests
{
    private readonly CardExpiryDateValidator _validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(13)]
    public void Validate_InvalidMonth_ReturnsValidationError(int month)
    {
        var value = (Month: (int?)month, Year: (int?)2030);

        var results = _validator.Validate(value).ToList();

        results.Should().HaveCount(1);
        results.First().MemberNames.Should().ContainSingle(CardExpiryDateValidator.MonthProperty);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void Validate_ValidMonth_DoesNotReturnMonthValidationError(int month)
    {
        var value = (Month: (int?)month, Year: (int?)2030);

        var results = _validator.Validate(value).ToList();

        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1990)]
    public void Validate_YearInPast_ReturnsYearValidationError(int year)
    {
        var value = (Month: (int?)12, Year: (int?)year);

        var results = _validator.Validate(value).ToList();

        results.First().MemberNames.Should().Contain(CardExpiryDateValidator.YearProperty);
    }

    [Fact]
    public void Validate_CurrentYearPastMonth_ReturnsDateValidationError()
    {
        var currentYear = DateTimeOffset.Now.Year;
        var pastMonth = Math.Max(1, DateTimeOffset.Now.Month - 1);
        var value = (Month: (int?)pastMonth, Year: (int?)currentYear);

        var results = _validator.Validate(value).ToList();

        results.First().MemberNames.Should()
            .Contain([CardExpiryDateValidator.MonthProperty, CardExpiryDateValidator.YearProperty]);
    }

    [Fact]
    public void Validate_CurrentYearCurrentMonth_ReturnsDateValidationError()
    {
        var currentYear = DateTimeOffset.Now.Year;
        var currentMonth = DateTimeOffset.Now.Month;
        var value = (Month: (int?)currentMonth, Year: (int?)currentYear);

        var results = _validator.Validate(value).ToList();

        results.First().MemberNames.Should()
            .Contain([CardExpiryDateValidator.MonthProperty, CardExpiryDateValidator.YearProperty]);
    }

    [Fact]
    public void Validate_FutureDate_ReturnsNoValidationErrors()
    {
        var currentYear = DateTimeOffset.Now.Year + 1;
        var value = (Month: (int?)12, Year: (int?)currentYear);

        var results = _validator.Validate(value).ToList();

        results.Should().BeEmpty();
    }
}