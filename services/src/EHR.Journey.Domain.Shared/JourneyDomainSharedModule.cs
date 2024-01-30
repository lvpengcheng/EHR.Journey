using EHR.Journey.BasicManagement;
using EHR.Journey.BasicManagement.Localization;
using EHR.Journey.Core;
using EHR.Journey.LanguageManagement;

namespace EHR.Journey
{
    [DependsOn(
        typeof(JourneyCoreModule),
        typeof(BasicManagementDomainSharedModule),
        typeof(DataDictionaryManagementDomainSharedModule),
        typeof(NotificationManagementDomainSharedModule),
        typeof(LanguageManagementDomainSharedModule)
    )]
    public class JourneyDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            JourneyGlobalFeatureConfigurator.Configure();
            JourneyModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<JourneyDomainSharedModule>(JourneyDomainSharedConsts.NameSpace); });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<JourneyResource>(JourneyDomainSharedConsts.DefaultCultureName)
                    .AddVirtualJson("/Localization/Journey")
                    .AddBaseTypes(typeof(BasicManagementResource))
                    .AddBaseTypes(typeof(AbpTimingResource));

                options.DefaultResourceType = typeof(JourneyResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace(JourneyDomainSharedConsts.NameSpace, typeof(JourneyResource)); });
        }
    }
}