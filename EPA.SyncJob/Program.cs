using EPA.SyncJob;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using HRM.Website.Middlewares;

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
if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ErrorHandler>();
    app.UseHsts();
}
app.Run(async context =>
{
    var request = context.Request.Path.HasValue
        ? context.Request.Path.Value
        : string.Empty;

    await context.Response.WriteAsync($"EPA Auto Jobs. Request Path = {request}");
});

await app.RunAsync();