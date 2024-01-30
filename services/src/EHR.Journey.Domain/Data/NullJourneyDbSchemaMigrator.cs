namespace EHR.Journey.Data
{
    /* This is used if database provider does't define
     * IJourneyDbSchemaMigrator implementation.
     */
    public class NullJourneyDbSchemaMigrator : IJourneyDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}