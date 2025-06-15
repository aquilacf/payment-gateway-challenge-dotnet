using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Data;

public sealed class PaymentRepository : IRepository<Payment>
{
    private readonly Dictionary<Guid, Payment> _payments = [];

    public void Save(Payment entity) => _payments.Add(entity.Id, entity);
    public Payment? Get(Guid paymentId) => _payments.GetValueOrDefault(paymentId);
}