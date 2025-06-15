using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Endpoints.Payments.Mappers;
using PaymentGateway.Api.Endpoints.Payments.Models;
using PaymentGateway.Application;
using PaymentGateway.Application.Features.Payments.Queries;

namespace PaymentGateway.Api.Endpoints.Payments;

internal static class PaymentDetailsEndpoint
{
    internal static async Task<Results<Ok<PaymentResponse>, NotFound>> HandlerAsync(
        [FromRoute] Guid id,
        // Mediator would go here
        IApplicationHandler<PaymentDetailsQuery, PaymentDetailsQueryResult> paymentDetailsHandler)
    {
        var result = await paymentDetailsHandler.HandleAsync(new PaymentDetailsQuery(id));
        return result.Payment is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(result.Payment.ToPaymentResponse());
    }
}