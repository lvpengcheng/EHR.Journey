namespace EHR.Journey.Data
{
    public interface IJourneyDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
