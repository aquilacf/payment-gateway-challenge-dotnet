using PaymentGateway.Application.Features.Payments.Commands;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.Features.Payments.Handlers;

public class PaymentRejectCommandHandler(IRepository<Payment> paymentRepository)
    : IApplicationHandler<PaymentRejectCommand, PaymentRejectCommandResult>
{
    public Task<PaymentRejectCommandResult> HandleAsync(PaymentRejectCommand request)
    {
        var entity = new Payment
        {
            Id = Guid.NewGuid(),
            Status = PaymentStatus.Rejected,
            CardNumberLastFour = request.CardNumber?.Substring(request.CardNumber.Length - 4),
            ExpiryMonth = request.ExpiryMonth,
            ExpiryYear = request.ExpiryYear,
            Currency = request.Currency,
            Amount = request.Amount,
            Errors = request.Errors,
        };

        paymentRepository.Save(entity);

        return Task.FromResult(new PaymentRejectCommandResult(entity));
    }
}