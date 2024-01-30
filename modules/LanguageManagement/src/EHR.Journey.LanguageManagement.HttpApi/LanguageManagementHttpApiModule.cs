using Localization.Resources.AbpUi;
using EHR.Journey.LanguageManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EHR.Journey.LanguageManagement
{
    [DependsOn(
        typeof(LanguageManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class LanguageManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(LanguageManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<LanguageManagementResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
