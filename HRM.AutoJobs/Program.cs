using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using HRM.AutoJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpRaven.Data.Context;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHostedService<Worker>();
services.AddHttpClient();
services.AddCoreServices();
services.AddFanexSql(configuration);
services.ConfigureAppSettings(configuration);
services.AddFanexLogSentry(configuration);
//services.AddRedis(configuration);
//services.AddRedisSession(configuration);
services.AddJobsServices();
services.AddHealthChecks()
    .AddCheck<CustomHealthCheck>("custom_check");

var host = builder.Build();

await host.RunAsync();

public class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("Healthy"));
    }
}