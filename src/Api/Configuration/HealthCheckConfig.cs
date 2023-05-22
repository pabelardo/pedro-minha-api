using Api.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Configuration;

public static class HealthCheckConfig
{
    public static void AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(name: "sqlserver", connectionString: configuration.GetConnectionString("DefaultConnection"),
                tags: new[] { "db", "data" })
            .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")),
                tags: new[] { "db", "data" })
            .AddGCInfoCheck("GCInfo", HealthStatus.Unhealthy, new[] { "gc" });

        services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(5);
                options.MaximumHistoryEntriesPerEndpoint(10);
            })
            //.AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"))
            .AddInMemoryStorage();
    }
}