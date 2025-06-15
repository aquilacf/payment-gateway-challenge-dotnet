using PaymentGateway.Application.Features.Payments.Queries;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.Features.Payments.Handlers;

public sealed class PaymentDetailsQueryHandler(IRepository<Payment> paymentRepository)
    : IApplicationHandler<PaymentDetailsQuery, PaymentDetailsQueryResult>
{
    public Task<PaymentDetailsQueryResult> HandleAsync(PaymentDetailsQuery request)
    {
        return Task.FromResult(new PaymentDetailsQueryResult(paymentRepository.Get(request.Id)));
    }
}