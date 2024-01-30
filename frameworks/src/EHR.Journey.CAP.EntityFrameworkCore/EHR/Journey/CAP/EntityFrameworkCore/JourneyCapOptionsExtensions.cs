// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class JourneyCapOptionsExtensions
    {
        public static CapOptions SetCapDbConnectionString(this CapOptions options, string dbConnectionString)
        {
            options.RegisterExtension(new JourneyEfCoreDbContextCapOptionsExtension
            {
                CapUsingDbConnectionString = dbConnectionString
            });
            
            return options;
        }
    }
}
