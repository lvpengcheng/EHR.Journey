namespace EHR.Journey.CAP;

public static class JourneyCapServiceCollectionExtensions
{
    public static ServiceConfigurationContext AddAbpCap(this ServiceConfigurationContext context, Action<CapOptions> capAction)
    {
        context.Services.Replace(ServiceDescriptor.Transient<IUnitOfWork, JourneyCapUnitOfWork>());
        context.Services.Replace(ServiceDescriptor.Transient<UnitOfWork, JourneyCapUnitOfWork>());
        context.Services.AddTransient<JourneyCapUnitOfWork>();
        context.Services.AddCap(capAction);
        return context;
    }
}