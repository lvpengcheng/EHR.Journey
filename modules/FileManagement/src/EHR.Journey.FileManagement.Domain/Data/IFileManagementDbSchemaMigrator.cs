namespace EHR.Journey.FileManagement.Data;

public interface IFileManagementDbSchemaMigrator
{
    Task MigrateAsync();
}