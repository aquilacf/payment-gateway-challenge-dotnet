using PaymentGateway.Api.Exceptions;
using PaymentGateway.Domain;

namespace PaymentGateway.Api;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddPresentation(this IServiceCollection serviceCollection)
    {
        serviceCollection.ConfigureHttpJsonOptions(options =>
            Constants.ApplyDefaultSettings(options.SerializerOptions));

        serviceCollection
            .AddExceptionHandler<ExceptionHandler>()
            .AddProblemDetails()
            .AddOpenApi();

        return serviceCollection;
    }
}