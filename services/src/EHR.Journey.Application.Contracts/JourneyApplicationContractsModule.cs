namespace EHR.Journey
{
    [DependsOn(
        typeof(JourneyDomainSharedModule),
        typeof(AbpObjectExtendingModule),
        typeof(BasicManagementApplicationContractsModule),
        typeof(DataDictionaryManagementApplicationContractsModule),
        typeof(LanguageManagementApplicationContractsModule)
    )]
    public class JourneyApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            JourneyDtoExtensions.Configure();
        }
    }
}
