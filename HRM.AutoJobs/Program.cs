using Fanex.Logging.Sentry.RavenClient;
using HRM.AutoJobs;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHostedService<Worker>();
services.AddHttpClient();
services.AddCoreServices();
services.ConfigureAppSettings(configuration);
services.AddFanexLogSentry(configuration);
//services.AddRedis(configuration);
//services.AddRedisSession(configuration);
services.AddJobsServices();

var host = builder.Build();
await host.RunAsync();
