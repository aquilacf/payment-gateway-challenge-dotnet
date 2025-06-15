using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;
using PaymentGateway.Infrastructure.Services;
using PaymentGateway.Infrastructure.Validators;

namespace PaymentGateway.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfratructure(this IServiceCollection serviceCollection)
    {
        // Resilience is configured on service defaults
        serviceCollection.AddHttpClient<IAcquiringBankService, MountebankService>("bank", static options =>
        {
            options.BaseAddress = new("http://_bank.bank-simulator"); // Uses service discovery to find out the URL
        });

        return serviceCollection
            .AddValidators()
            .AddSingleton<IRepository<Payment>, PaymentRepository>();
    }

    private static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<CreditCardValidator>()
            .AddSingleton<CardExpiryDateValidator>()
            .AddSingleton<CvvValidator>()
            .AddSingleton<CurrencyValidator>();
    }
}