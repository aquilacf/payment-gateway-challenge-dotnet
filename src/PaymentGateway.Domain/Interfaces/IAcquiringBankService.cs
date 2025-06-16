namespace PaymentGateway.Domain.Interfaces;

public interface IAcquiringBankService
{
    Task<AcquiringBankResponse?> ProcessAsync(AcquiringBankRequest request);
}

public sealed record AcquiringBankRequest
{
    public required string CardNumber { get; init; }
    public required string ExpiryDate { get; init; }
    public required string Currency { get; init; }
    public required int Amount { get; init; }
    public required string Cvv { get; init; }
}

public sealed record AcquiringBankResponse
{
    public required bool Authorized { get; init; }
    public required string AuthorizationCode { get; init; }
}
