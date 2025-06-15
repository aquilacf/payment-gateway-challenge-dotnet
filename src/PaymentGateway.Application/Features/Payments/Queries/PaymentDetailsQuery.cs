using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Features.Payments.Queries;

public sealed record PaymentDetailsQuery(Guid Id);

public sealed record PaymentDetailsQueryResult(Payment? Payment);