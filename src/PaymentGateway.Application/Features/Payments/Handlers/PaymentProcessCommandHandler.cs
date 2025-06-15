using PaymentGateway.Application.Features.Payments.Commands;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.Features.Payments.Handlers;

public class PaymentProcessCommandHandler(
    IRepository<Payment> paymentRepository,
    IAcquiringBankService bankService) : IApplicationHandler<PaymentProcessCommand, PaymentProcessCommandResult>
{
    public async Task<PaymentProcessCommandResult> HandleAsync(PaymentProcessCommand request)
    {
        var bankResponse = await bankService.ProcessAsync(new AcquiringBankRequest
        {
            CardNumber = request.CardNumber,
            ExpiryDate = $"{request.ExpiryMonth}/{request.ExpiryYear}",
            Currency = request.Currency,
            Amount = request.Amount,
            Cvv = request.Cvv
        });
        if (bankResponse is null)
        {
            return new PaymentProcessCommandResult(null);
        }

        var entity = new Payment
        {
            Id = Guid.NewGuid(),
            Status = bankResponse.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined,
            CardNumberLastFour = request.CardNumber.Substring(request.CardNumber.Length - 4),
            ExpiryMonth = request.ExpiryMonth,
            ExpiryYear = request.ExpiryYear,
            Currency = request.Currency,
            Amount = request.Amount,
            AuthorizationCode = bankResponse.AuthorizationCode
        };

        paymentRepository.Save(entity);

        return new PaymentProcessCommandResult(entity);
    }
}