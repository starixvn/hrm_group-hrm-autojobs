using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using HRM.AutoJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
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

var app = builder.Build();
app.Run(async context =>
{
    var request = context.Request.Path.HasValue
        ? context.Request.Path.Value
        : string.Empty;

    await context.Response.WriteAsync($"HRM Auto Jobs. Request Path = {request}");
});

await app.RunAsync();
