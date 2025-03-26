using EPA.SyncJob;
using Fanex.Data.SqlServer;
using Microsoft.Extensions.Hosting;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.Extensions.Logging;
using SharpRaven.Data.Context;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddCoreServices(configuration);
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);
//services.AddRedis(configuration);
//services.AddRedisSession(configuration);
services.AddJobsServices();


var app = builder.Build();
app.Run(async context =>
{
    var request = context.Request.Path.HasValue
        ? context.Request.Path.Value
        : string.Empty;

    await context.Response.WriteAsync($"EPA Auto Jobs. Request Path = {request}");
});

await app.RunAsync();