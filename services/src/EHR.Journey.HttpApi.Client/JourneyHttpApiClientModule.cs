using EHR.Journey.BasicManagement;
using EHR.Journey.LanguageManagement;
using EHR.Journey.NotificationManagement;

namespace EHR.Journey
{
    [DependsOn(
        typeof(JourneyApplicationContractsModule),
        typeof(BasicManagementHttpApiClientModule),
        typeof(DataDictionaryManagementHttpApiClientModule),
        typeof(NotificationManagementHttpApiClientModule),
        typeof(LanguageManagementHttpApiClientModule)
    )]
    public class JourneyHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(JourneyApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
