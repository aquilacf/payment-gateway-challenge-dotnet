using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Application.Features.Payments.Commands;
using PaymentGateway.Application.Features.Payments.Handlers;
using PaymentGateway.Application.Features.Payments.Queries;

namespace PaymentGateway.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IApplicationHandler<PaymentRejectCommand, PaymentRejectCommandResult>, PaymentRejectCommandHandler>()
            .AddScoped<IApplicationHandler<PaymentProcessCommand, PaymentProcessCommandResult>, PaymentProcessCommandHandler>()
            .AddScoped<IApplicationHandler<PaymentDetailsQuery, PaymentDetailsQueryResult>, PaymentDetailsQueryHandler>();

        return serviceCollection;
    }
}