namespace PaymentGateway.Domain.Interfaces;

// Depending on the use case, I would split the IRepository into IReadOnlyRepository and IWriteRepository: event sourcing to two different dynamodbs, writes go to events db and reads come from views db
public interface IRepository<TEntity>
{
    void Save(TEntity entity);
    TEntity? Get(Guid paymentId);
}