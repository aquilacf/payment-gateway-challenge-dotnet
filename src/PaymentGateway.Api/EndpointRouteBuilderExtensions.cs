using PaymentGateway.Api.Endpoints.Payments;

namespace PaymentGateway.Api;

internal static class EndpointRouteBuilderExtensions
{
    private const string PaymentEndpointGroup = "payments";

    internal static void MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup(PaymentEndpointGroup);

        group
            .MapPost("", PaymentRequestEndpoint.HandlerAsync)
            .WithName(nameof(PaymentRequestEndpoint));

        group
            .MapGet("{id:guid}", PaymentDetailsEndpoint.HandlerAsync)
            .WithName(nameof(PaymentDetailsEndpoint));
    }
}