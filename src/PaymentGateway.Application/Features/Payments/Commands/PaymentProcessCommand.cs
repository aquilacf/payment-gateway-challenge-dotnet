using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Features.Payments.Commands;

public sealed record PaymentProcessCommand(
    string CardNumber,
    int ExpiryMonth,
    int ExpiryYear,
    string Currency,
    int Amount,
    string Cvv);

public sealed record PaymentProcessCommandResult(Payment? Payment);