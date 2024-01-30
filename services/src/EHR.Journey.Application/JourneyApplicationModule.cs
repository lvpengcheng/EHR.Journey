using Employee;

namespace EHR.Journey
{
    [DependsOn(
        typeof(JourneyDomainModule),
        typeof(JourneyApplicationContractsModule),
        typeof(BasicManagementApplicationModule),
        typeof(DataDictionaryManagementApplicationModule),
        typeof(NotificationManagementApplicationModule),
        typeof(LanguageManagementApplicationModule),
        typeof(JourneyFreeSqlModule)
    )]
    public class JourneyApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<JourneyApplicationModule>(); });
        }
    }
}