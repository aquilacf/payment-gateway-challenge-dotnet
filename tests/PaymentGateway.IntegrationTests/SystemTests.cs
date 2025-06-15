using FluentAssertions;

namespace PaymentGateway.IntegrationTests;

public sealed class SystemTests(ServerFixture fixture) : IClassFixture<ServerFixture>
{
    [Fact]
    public async Task HealthEndpoint_Should_Return_Ok()
    {
        var httpClient = await fixture.CreateHttpClient("api");

        var response = await httpClient.GetAsync("/_system/health", TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AliveEndpoint_Should_Return_Ok()
    {
        var httpClient = await fixture.CreateHttpClient("api");

        var response = await httpClient.GetAsync("/_system/alive", TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}