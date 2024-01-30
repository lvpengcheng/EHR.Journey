namespace EHR.Journey.ElasticSearch;

[DependsOn(typeof(AbpAutofacModule))]
public class JourneyElasticSearchModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<JourneyElasticSearchOptions>(context.Services.GetConfiguration().GetSection("ElasticSearch"));
    }
}