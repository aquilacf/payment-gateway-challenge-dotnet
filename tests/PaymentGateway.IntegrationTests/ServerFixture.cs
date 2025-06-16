using Aspire.Hosting;

using Microsoft.Extensions.Logging;

namespace PaymentGateway.IntegrationTests;

public sealed class ServerFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    private ResourceNotificationService? _notificationService;
    private HttpClient? _httpClient;

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(5);

    public async ValueTask InitializeAsync()
    {
        var cancellationToken = new CancellationTokenSource(DefaultTimeout).Token;
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.PaymentGateway_AppHost>(cancellationToken);

        builder.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            logging.AddFilter(builder.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
        });

        builder.Services.ConfigureHttpClientDefaults(options =>
        {
            options.AddStandardResilienceHandler(x => x.TotalRequestTimeout.Timeout = DefaultTimeout);
        });

        builder.Services.Configure<DistributedApplicationOptions>(options =>
        {
            options.DisableDashboard = true;
        });

        _app = await builder.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        _notificationService = _app.Services.GetService<ResourceNotificationService>();

        await _app.StartAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async Task<HttpClient> CreateHttpClient(string applicationName)
    {
        if (_httpClient is not null)
        {
            return _httpClient;
        }

        _httpClient = _app!.CreateHttpClient(applicationName);

        await _notificationService!
            .WaitForResourceAsync(applicationName, KnownResourceStates.Running)
            .WaitAsync(DefaultTimeout);

        return _httpClient;
    }
}