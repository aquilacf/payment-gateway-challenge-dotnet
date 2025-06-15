using System.Net.Http.Json;
using System.Text.Json;

using FluentAssertions;

using PaymentGateway.Api.Endpoints.Payments.Models;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Enums;
using PaymentGateway.IntegrationTests.TheoryData;

namespace PaymentGateway.IntegrationTests;

public sealed class PaymentLifecycleTests(ServerFixture fixture) : IClassFixture<ServerFixture>
{
    public readonly Random Random = new();

    [Theory]
    [ClassData(typeof(ValidPaymentRequestTestData))]
    internal async Task Valid_PaymentRequest_Retrieves_Details(
        PaymentRequest paymentRequest,
        PaymentStatus status)
    {
        var httpClient = await fixture.CreateHttpClient("api");
        var paymentRequestResponse = await httpClient.PostAsJsonAsync("/payments", paymentRequest,
            Constants.DefaultJsonSerializerOptions, cancellationToken: TestContext.Current.CancellationToken);

        paymentRequestResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var paymentRequestResponseBody = await paymentRequestResponse.Content.ReadFromJsonAsync<PaymentResponse>(
            Constants.DefaultJsonSerializerOptions, TestContext.Current.CancellationToken);

        paymentRequestResponseBody.Should().NotBeNull();

        var responseDetailsResponse = await httpClient.GetAsync(
            $"/payments/{paymentRequestResponseBody.Id}", TestContext.Current.CancellationToken);

        responseDetailsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var paymentDetailsResponseBody =
            await responseDetailsResponse.Content.ReadFromJsonAsync<PaymentResponse>(
                Constants.DefaultJsonSerializerOptions, TestContext.Current.CancellationToken);

        paymentDetailsResponseBody.Should().NotBeNull();

        paymentDetailsResponseBody.Status.Should().Be(status);
    }

    [Fact]
    internal async Task Valid_PaymentRequest_Retrieves_Details2()
    {
        var paymentRequest = new PaymentRequest
        {
            ExpiryYear = Random.Next(2026, 2030),
            ExpiryMonth = Random.Next(1, 12),
            Amount = Random.Next(1, 10000),
            CardNumber = "11111111111110",
            Cvv = Random.Next(111, 9999).ToString(),
            Currency = "GBP"
        };

        var httpClient = await fixture.CreateHttpClient("api");
        var paymentRequestResponse = await httpClient.PostAsJsonAsync("/payments", paymentRequest,
            Constants.DefaultJsonSerializerOptions, cancellationToken: TestContext.Current.CancellationToken);

        paymentRequestResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    internal async Task Random_Id_Should_Return_NotFound()
    {
        var httpClient = await fixture.CreateHttpClient("api");

        var response =
            await httpClient.GetAsync($"/api/Payments/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    internal async Task Unrelated_Payload_Should_ReturnBadRequest()
    {
        var httpClient = await fixture.CreateHttpClient("api");
        var paymentRequestResponse = await httpClient.PostAsJsonAsync("/payments", new { Email = "", Password = "" },
            Constants.DefaultJsonSerializerOptions, cancellationToken: TestContext.Current.CancellationToken);

        paymentRequestResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [ClassData(typeof(InvalidPaymentRequestTestData))]
    internal async Task Invalid_PaymentRequest_Should_ReturnBadRequest_And_Rejected(
        PaymentRequest paymentRequest,
        string[] errorCodes)
    {
        var httpClient = await fixture.CreateHttpClient("api");
        var paymentRequestResponse = await httpClient.PostAsJsonAsync("/payments", paymentRequest,
            Constants.DefaultJsonSerializerOptions, cancellationToken: TestContext.Current.CancellationToken);

        paymentRequestResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var paymentRequestResponseBody = await paymentRequestResponse.Content.ReadFromJsonAsync<JsonElement>(
            Constants.DefaultJsonSerializerOptions, TestContext.Current.CancellationToken);

        var errors = paymentRequestResponseBody.GetProperty("errors");

        errors.Should().NotBeNull();
        errors.GetPropertyCount().Should().Be(errorCodes.Length);

        foreach (var errorCode in errorCodes)
        {
            errors.GetProperty(errorCode).Should().NotBeNull();
        }
    }
}