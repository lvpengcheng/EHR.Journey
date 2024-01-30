namespace EHR.Journey.CAP;

public interface IJourneyCapTransactionApiFactory
{
    Type TransactionApiType { get; }
    
    ITransactionApi Create(ITransactionApi originalApi);
}