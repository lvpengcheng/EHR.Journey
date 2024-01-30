namespace EHR.Journey.Cli;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp.IdentityModel", LogEventLevel.Information)
            .MinimumLevel.Override("Volo.Abp.Cli", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(Path.Combine(CliPaths.Log, "lion.abp-pro-cli-logs.txt"))
            .WriteTo.Console()
            .CreateLogger();
        using var application = await AbpApplicationFactory.CreateAsync<JourneyCliModule>(
            options =>
            {
                options.UseAutofac();
                options.Services.AddLogging(c => c.AddSerilog());
            });
        await application.InitializeAsync();

        await application.ServiceProvider
            .GetRequiredService<CliService>()
            .RunAsync(args);

        await application.ShutdownAsync();

        Log.CloseAndFlush();
    }
}