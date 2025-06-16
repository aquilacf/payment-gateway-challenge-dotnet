using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Entities;

public sealed class Payment
{
    public required Guid Id { get; set; }
    public required PaymentStatus Status { get; set; }
    public string? CardNumberLastFour { get; set; }
    public int? ExpiryMonth { get; set; }
    public int? ExpiryYear { get; set; }
    public string? Currency { get; set; }
    public int? Amount { get; set; }

    public string? AuthorizationCode { get; set; }
    // The requirements imply that even rejected payments should be retrieved later one, so bad requests errors must be stored?
    public Dictionary<string, string?[]>? Errors { get; set; }
}