namespace EHR.Journey.CAP.EntityFrameworkCore;

public class JourneyJourneyCapDbProviderInfoProvider : IJourneyCapDbProviderInfoProvider, ITransientDependency
{
    protected ConcurrentDictionary<string, JourneyCapDbProviderInfo> CapDbProviderInfos { get; set; } = new();

    public virtual JourneyCapDbProviderInfo GetOrNull(string dbProviderName)
    {
        return CapDbProviderInfos.GetOrAdd(dbProviderName, InternalGetOrNull);
    }
    
    protected virtual JourneyCapDbProviderInfo InternalGetOrNull(string databaseProviderName)
    {
        switch (databaseProviderName)
        {
            case "Microsoft.EntityFrameworkCore.SqlServer":
                return new JourneyCapDbProviderInfo(
                    "DotNetCore.CAP.SqlServerCapTransaction, DotNetCore.CAP.SqlServer",
                    "Microsoft.EntityFrameworkCore.Storage.CapEFDbTransaction, DotNetCore.CAP.SqlServer");
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                return new JourneyCapDbProviderInfo(
                    "DotNetCore.CAP.PostgreSqlCapTransaction, DotNetCore.CAP.PostgreSql",
                    "Microsoft.EntityFrameworkCore.Storage.CapEFDbTransaction, DotNetCore.CAP.PostgreSQL");
            case "Pomelo.EntityFrameworkCore.MySql":
                return new JourneyCapDbProviderInfo(
                    "DotNetCore.CAP.MySqlCapTransaction, DotNetCore.CAP.MySql",
                    "Microsoft.EntityFrameworkCore.Storage.CapEFDbTransaction, DotNetCore.CAP.MySql");
            case "Oracle.EntityFrameworkCore":
            case "Devart.Data.Oracle.Entity.EFCore":
                return new JourneyCapDbProviderInfo(
                    "DotNetCore.CAP.OracleCapTransaction, DotNetCore.CAP.Oracle",
                    "Microsoft.EntityFrameworkCore.Storage.CapEFDbTransaction, DotNetCore.CAP.Oracle");
            case "Microsoft.EntityFrameworkCore.Sqlite":
                return new JourneyCapDbProviderInfo(
                    "DotNetCore.CAP.SqliteCapTransaction, DotNetCore.CAP.Sqlite",
                    "Microsoft.EntityFrameworkCore.Storage.CapEFDbTransaction, DotNetCore.CAP.Sqlite");
            case "Microsoft.EntityFrameworkCore.InMemory":
                return new JourneyCapDbProviderInfo(
                    "DotNetCore.CAP.InMemoryCapTransaction, DotNetCore.CAP.InMemoryStorage",
                    "Microsoft.EntityFrameworkCore.Storage.CapEFDbTransaction, DotNetCore.CAP.InMemoryStorage");
            default:
                return null;
        }
    }
}