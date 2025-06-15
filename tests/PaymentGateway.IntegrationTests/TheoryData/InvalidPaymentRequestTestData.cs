using PaymentGateway.Api.Endpoints.Payments.Models;

namespace PaymentGateway.IntegrationTests.TheoryData;

internal sealed class InvalidPaymentRequestTestData : TheoryData<PaymentRequest, string[]>
{
    public InvalidPaymentRequestTestData()
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
                Currency = "GBPx"
            },
            ["currency"]);
        Add(
            new PaymentRequest
            {
                ExpiryYear = random.Next(2026, 2030),
                ExpiryMonth = random.Next(1, 12),
                Amount = random.Next(1, 10000),
                CardNumber = "11111111111111",
                Cvv = "12345",
                Currency = "GBP"
            }, ["cvv"]);

        Add(
            new PaymentRequest
            {
                ExpiryYear = random.Next(2026, 2030),
                ExpiryMonth = random.Next(1, 12),
                Amount = random.Next(1, 10000),
                CardNumber = "1234",
                Cvv = random.Next(111, 9999).ToString(),
                Currency = "GBP"
            }, ["card_number"]);

        Add(
            new PaymentRequest
            {
                ExpiryYear = random.Next(2026, 2030),
                ExpiryMonth = random.Next(1, 12),
                Amount = -10,
                CardNumber = "11111111111111",
                Cvv = random.Next(111, 9999).ToString(),
                Currency = "GBP"
            }, ["amount"]);

        Add(
            new PaymentRequest
            {
                ExpiryYear = random.Next(2026, 2030),
                ExpiryMonth = 13,
                Amount = random.Next(1, 10000),
                CardNumber = "11111111111111",
                Cvv = random.Next(111, 9999).ToString(),
                Currency = "GBP"
            }, ["expiry_month"]);
        
        Add(
            new PaymentRequest
            {
                ExpiryYear = -123,
                ExpiryMonth = random.Next(1, 12),
                Amount = random.Next(1, 10000),
                CardNumber = "11111111111111",
                Cvv = random.Next(111, 9999).ToString(),
                Currency = "GBP"
            }, ["expiry_year"]);

        Add(
            new PaymentRequest
            {
                ExpiryYear = -123,
                ExpiryMonth = 13,
                Amount = -10,
                CardNumber = "1234",
                Cvv = "12345",
                Currency = "GBPx"
            }, ["currency", "cvv", "card_number", "amount", "expiry_month", "expiry_year",]);
    }
}