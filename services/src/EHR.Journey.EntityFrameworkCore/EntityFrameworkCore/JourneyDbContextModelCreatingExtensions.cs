namespace EHR.Journey.EntityFrameworkCore
{
    public static class JourneyDbContextModelCreatingExtensions
    {
        public static void ConfigureJourney(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(JourneyConsts.DbTablePrefix + "YourEntities", JourneyConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}