using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Services;

public sealed class MountebankService(ILogger<MountebankService> logger, HttpClient bankClient) : IAcquiringBankService
{
    public async Task<AcquiringBankResponse?> ProcessAsync(AcquiringBankRequest request)
    {
        var response = await bankClient.PostAsJsonAsync("/payments", request, Constants.DefaultJsonSerializerOptions);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("success");
            var content =
                await response.Content.ReadFromJsonAsync<AcquiringBankResponse>(Constants.DefaultJsonSerializerOptions);

            return content;
        }

        logger.LogError("error");
        return null;
    }
}