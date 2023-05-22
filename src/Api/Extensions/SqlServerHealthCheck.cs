using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Extensions;

public class SqlServerHealthCheck : IHealthCheck
{
    private readonly string _connection;

    public SqlServerHealthCheck(string connection)
    {
        _connection = connection;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            await using var connection = new SqlConnection(_connection);

            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = "select count(id) from produtos";

            return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) > 0
                ? HealthCheckResult.Healthy("Verifica se existem produtos no banco.")
                : HealthCheckResult.Unhealthy("Não existem produtos no banco.");
        }
        catch (Exception)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}