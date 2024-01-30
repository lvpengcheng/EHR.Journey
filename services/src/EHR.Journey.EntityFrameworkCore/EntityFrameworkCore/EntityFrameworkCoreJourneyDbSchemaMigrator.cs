namespace EHR.Journey.EntityFrameworkCore
{
    public class EntityFrameworkCoreJourneyDbSchemaMigrator
        : IJourneyDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreJourneyDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the JourneyMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<JourneyDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}