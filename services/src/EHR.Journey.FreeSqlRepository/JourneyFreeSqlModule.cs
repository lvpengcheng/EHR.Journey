namespace EHR.Journey.FreeSqlRepository;

public class JourneyFreeSqlModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var connectionString = configuration.GetConnectionString("Default");
        var freeSql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.MySql, connectionString)
            .Build();
        
        freeSql.Aop.CurdAfter += (s, e) =>
        {
            Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId};" +
                              $" FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");
        };
        
        context.Services.AddSingleton<IFreeSql>(freeSql);
    }
}