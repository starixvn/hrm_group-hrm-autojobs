using EPA.SyncJob;
using Fanex.Data.SqlServer;
using Microsoft.Extensions.Hosting;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var storedProcedurePath = configuration.GetSection("StoredProcedurePath").Value;

services.AddCoreServices(configuration);
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);
//services.AddRedis(configuration);
//services.AddRedisSession(configuration);
services.AddJobsServices();

var host = builder.Build();
await host.RunAsync();