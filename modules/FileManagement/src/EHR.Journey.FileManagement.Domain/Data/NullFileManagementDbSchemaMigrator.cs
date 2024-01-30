namespace EHR.Journey.FileManagement.Data;

public class NullFileManagementDbSchemaMigrator : IFileManagementDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}