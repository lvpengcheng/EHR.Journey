namespace EHR.Journey.FileManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(FileManagementDomainSharedModule)
)]
public class FileManagementDomainModule : AbpModule
{
}