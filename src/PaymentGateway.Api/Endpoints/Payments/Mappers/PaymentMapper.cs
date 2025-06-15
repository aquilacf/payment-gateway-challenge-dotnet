using PaymentGateway.Api.Endpoints.Payments.Models;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Api.Endpoints.Payments.Mappers;

internal static class PaymentMapper
{
    internal static PaymentResponse ToPaymentResponse(this Payment entity)
    {
        return new PaymentResponse
        {
            Id = entity.Id,
            Status = entity.Status,
            CardNumberLastFour = entity.CardNumberLastFour,
            ExpiryMonth = entity.ExpiryMonth,
            ExpiryYear = entity.ExpiryYear,
            Currency = entity.Currency,
            Amount = entity.Amount,
            AuthorizationCode = entity.AuthorizationCode,
            Errors = entity.Errors
        };
    }
}