namespace EHR.Journey.BasicManagement.Data
{
    public interface IJourneyDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
