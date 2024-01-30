namespace EHR.Journey.Cli;

[DependsOn(
    typeof(EHR.Journey.Cli.JourneyCliCoreModule),
    typeof(AbpAutofacModule)
)]
public class JourneyCliModule : AbpModule
{
}
