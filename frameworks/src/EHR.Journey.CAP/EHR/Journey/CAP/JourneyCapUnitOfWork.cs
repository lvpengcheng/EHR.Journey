namespace EHR.Journey.CAP;

[Dependency(TryRegister = true)]
public class JourneyCapUnitOfWork : UnitOfWork
{
    public ICapTransaction CapTransaction { get; protected set; }

    protected ICapPublisher CapPublisher { get; }

    public JourneyCapUnitOfWork(
        IServiceProvider serviceProvider,
        IUnitOfWorkEventPublisher unitOfWorkEventPublisher,
        IOptions<AbpUnitOfWorkDefaultOptions> options,
        ICapPublisher capPublisher)
        : base(serviceProvider, unitOfWorkEventPublisher, options)
    {
        CapPublisher = capPublisher;
    }

    public override void AddTransactionApi(string key, ITransactionApi api)
    {
        var factories = ServiceProvider.GetServices<IJourneyCapTransactionApiFactory>();

        var factory = factories.FirstOrDefault(x => x.TransactionApiType == api.GetType());

        if (factory is not null)
        {
            api = factory.Create(api);
            CapTransaction = CapPublisher.Transaction.Value;
        }
        
        base.AddTransactionApi(key, api);
    }

    public override ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory)
    {
        Check.NotNull(key, nameof(key));
        Check.NotNull(factory, nameof(factory));

        var transactionApi = FindTransactionApi(key);

        if (transactionApi is not null)
        {
            return transactionApi;
        }
        
        AddTransactionApi(key, factory());

        return FindTransactionApi(key);
    }
}