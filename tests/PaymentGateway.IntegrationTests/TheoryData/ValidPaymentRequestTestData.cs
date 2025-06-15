using PaymentGateway.Api.Endpoints.Payments.Models;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.IntegrationTests.TheoryData;

internal sealed class ValidPaymentRequestTestData : TheoryData<PaymentRequest, PaymentStatus>
{
    public ValidPaymentRequestTestData()
    {
        var random = new Random();

        Add(
            new PaymentRequest
            {
                ExpiryYear = random.Next(2026, 2030),
                ExpiryMonth = random.Next(1, 12),
                Amount = random.Next(1, 10000),
                CardNumber = "11111111111111",
                Cvv = random.Next(111, 9999).ToString(),
                Currency = "GBP"
            },
            PaymentStatus.Authorized);
        Add(
            new PaymentRequest
            {
                ExpiryYear = random.Next(2026, 2030),
                ExpiryMonth = random.Next(1, 12),
                Amount = random.Next(1, 10000),
                CardNumber = "11111111111112",
                Cvv = random.Next(111, 9999).ToString(),
                Currency = "GBP"
            }, PaymentStatus.Declined);
    }
}