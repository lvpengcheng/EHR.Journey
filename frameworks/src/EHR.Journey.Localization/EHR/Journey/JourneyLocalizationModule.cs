namespace EHR.Journey;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpLocalizationModule)
)]
public class JourneyLocalizationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<JourneyLocalizationModule>(JourneyLocalizationConsts.NameSpace); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<JourneyLocalizationResource>(JourneyLocalizationConsts.DefaultCultureName)
                .AddVirtualJson(JourneyLocalizationConsts.DefaultLocalizationResourceVirtualPath);

            options.DefaultResourceType = typeof(JourneyLocalizationResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace(JourneyLocalizationConsts.NameSpace, typeof(JourneyLocalizationResource)); });
    }
}