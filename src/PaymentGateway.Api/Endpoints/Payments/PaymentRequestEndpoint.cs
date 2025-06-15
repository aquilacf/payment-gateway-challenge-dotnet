using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Endpoints.Payments.Mappers;
using PaymentGateway.Api.Endpoints.Payments.Models;
using PaymentGateway.Application;
using PaymentGateway.Application.Features.Payments.Commands;

namespace PaymentGateway.Api.Endpoints.Payments;

internal static class PaymentRequestEndpoint
{
    internal static async Task<Results<Created<PaymentResponse>, BadRequest<PaymentResponse>, UnprocessableEntity>>
        HandlerAsync(
            [FromBody] PaymentRequest request,
            IServiceProvider serviceProvider,
            // Mediator would go here
            IApplicationHandler<PaymentRejectCommand, PaymentRejectCommandResult> paymentRejectHandler,
            IApplicationHandler<PaymentProcessCommand, PaymentProcessCommandResult> paymentProcessHandler)
    {
        //------- From the requirements, the merchant should also get details for rejected payments. Otherwise validation would be an endpoint filter

        var validationContext = new ValidationContext(request, serviceProvider, null);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errors = validationResults
                .GroupBy(x => x.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());
            var rejectionResult = await paymentRejectHandler.HandleAsync(new PaymentRejectCommand(errors,
                request.CardNumber, request.ExpiryMonth, request.ExpiryYear, request.Currency, request.Amount));
            return TypedResults.BadRequest(rejectionResult.Payment.ToPaymentResponse());
        }

        //---------

        var processResult = await paymentProcessHandler.HandleAsync(new PaymentProcessCommand(request.CardNumber!,
            request.ExpiryMonth!.Value, request.ExpiryYear!.Value, request.Currency!, request.Amount!.Value,
            request.Cvv!));

        return processResult.Payment is null
            ? TypedResults.UnprocessableEntity()
            : TypedResults.Created($"/payments/{processResult.Payment.Id}", processResult.Payment.ToPaymentResponse());
    }
}