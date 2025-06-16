using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Services;

public sealed class MountebankService(ILogger<MountebankService> logger, HttpClient bankClient) : IAcquiringBankService
{
    public async Task<AcquiringBankResponse?> ProcessAsync(AcquiringBankRequest request) // Would be wiser to pass cancellation token
    {
        var response = await bankClient.PostAsJsonAsync("/payments", request, Constants.DefaultJsonSerializerOptions);

        if (response.IsSuccessStatusCode)
        {
            var content =
                await response.Content.ReadFromJsonAsync<AcquiringBankResponse>(Constants.DefaultJsonSerializerOptions);

            return content;
        }

        logger.LogError("Acquiring bank has failed with {StatusCode}", response.StatusCode);
        return null;
    }
}