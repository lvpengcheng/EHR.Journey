namespace EHR.Journey.CAP;

public static class JourneyCapPublisherExtension
{
    public static IDisposable UseTransaction(this ICapPublisher capPublisher, ICapTransaction capTransaction)
    {
        var previousValue = capPublisher.Transaction.Value;
        capPublisher.Transaction.Value = capTransaction;
        return new DisposeAction(() => capPublisher.Transaction.Value = previousValue);
    }
}