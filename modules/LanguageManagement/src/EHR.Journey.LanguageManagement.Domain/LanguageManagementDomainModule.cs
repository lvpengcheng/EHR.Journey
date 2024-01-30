using Volo.Abp.SettingManagement;

namespace EHR.Journey.LanguageManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(LanguageManagementDomainSharedModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class LanguageManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.AddDynamicResource();
            });
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LanguageManagementDomainModule>();
            });
        }
    }
}