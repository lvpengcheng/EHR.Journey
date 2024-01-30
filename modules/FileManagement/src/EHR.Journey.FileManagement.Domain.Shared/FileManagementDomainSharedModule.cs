using EHR.Journey.Core;

namespace EHR.Journey.FileManagement;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(JourneyCoreModule)
)]
public class FileManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<FileManagementDomainSharedModule>(); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FileManagementResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/FileManagement");
        });

        Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace("FileManagement", typeof(FileManagementResource)); });
    }
}