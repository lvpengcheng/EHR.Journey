namespace EHR.Journey.CAP.EntityFrameworkCore;

public class EfCoreJourneyCapTransactionApiFactory : IJourneyCapTransactionApiFactory, ITransientDependency
{
    public Type TransactionApiType { get; } = typeof(EfCoreTransactionApi);

    protected readonly ICapPublisher Publisher;
    protected readonly JourneyEfCoreDbContextCapOptions Options;
    protected readonly IJourneyCapDbProviderInfoProvider JourneyCapDbProviderInfoProvider;
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    public EfCoreJourneyCapTransactionApiFactory(
        ICapPublisher publisher,
        IOptions<JourneyEfCoreDbContextCapOptions> options,
        IJourneyCapDbProviderInfoProvider abpProCapDbProviderInfoProvider,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Publisher = publisher;
        Options = options.Value;
        JourneyCapDbProviderInfoProvider = abpProCapDbProviderInfoProvider;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual ITransactionApi Create(ITransactionApi originalApi)
    {
        var efApi = (EfCoreTransactionApi)originalApi;

        var capTrans = CreateCapTransactionOrNull(efApi);

        return capTrans is null
            ? originalApi
            : new EfCoreTransactionApi(capTrans, efApi.StarterDbContext, CancellationTokenProvider);
    }

    protected virtual IDbContextTransaction CreateCapTransactionOrNull(EfCoreTransactionApi originalApi)
    {
        // TODO 通过数据库连接字符串判断数据库类型
        // if (Options.CapUsingDbConnectionString != originalApi.StarterDbContext.Database.GetConnectionString())
        // {
        //     return null;
        // }

        var dbProviderInfo = JourneyCapDbProviderInfoProvider.GetOrNull(originalApi.StarterDbContext.Database.ProviderName);

        if (dbProviderInfo?.CapTransactionType is null || dbProviderInfo.CapEfDbTransactionType is null)
        {
            return null;
        }

        var capTransactionType = dbProviderInfo.CapTransactionType;

        if (ActivatorUtilities.CreateInstance(Publisher.ServiceProvider, capTransactionType) is not CapTransactionBase capTransaction)
        {
            return null;
        }

        capTransaction.DbTransaction = originalApi.DbContextTransaction;
        capTransaction.AutoCommit = false;

        Publisher.Transaction.Value = capTransaction;

        return (IDbContextTransaction)Activator.CreateInstance(dbProviderInfo.CapEfDbTransactionType, capTransaction);
    }
}