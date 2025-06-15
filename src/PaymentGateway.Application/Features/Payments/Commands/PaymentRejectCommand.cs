using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Features.Payments.Commands;

public sealed record PaymentRejectCommand(
    Dictionary<string, string?[]> Errors,
    string? CardNumber,
    int? ExpiryMonth,
    int? ExpiryYear,
    string? Currency,
    int? Amount);

public record PaymentRejectCommandResult(Payment Payment);
