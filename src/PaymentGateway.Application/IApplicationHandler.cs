namespace PaymentGateway.Application;

public interface IApplicationHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}