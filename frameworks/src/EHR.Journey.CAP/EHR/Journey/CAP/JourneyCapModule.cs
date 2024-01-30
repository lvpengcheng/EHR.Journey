namespace EHR.Journey.CAP;

[DependsOn(
    typeof(AbpEventBusModule), 
    typeof(JourneyLocalizationModule),
    typeof(AbpUnitOfWorkModule))]
public class JourneyCapModule : AbpModule
{
}