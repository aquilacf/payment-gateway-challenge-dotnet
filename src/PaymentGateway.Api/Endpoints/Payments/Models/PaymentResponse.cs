using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Api.Endpoints.Payments.Models;

public sealed record PaymentResponse
{
    public required Guid Id { get; init; }
    public required PaymentStatus Status { get; init; }
    public required string? CardNumberLastFour { get; init; }
    public required int? ExpiryMonth { get; init; }
    public required int? ExpiryYear { get; init; }
    public required string? Currency { get; init; }
    public required int? Amount { get; init; }
    public string? AuthorizationCode { get; init; }
    public Dictionary<string, string?[]>? Errors { get; init; }
}