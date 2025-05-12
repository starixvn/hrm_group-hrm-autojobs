using EPA.SyncJob;
using EPA.SyncJob.Middlewares;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddCoreServices(configuration);
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);

services.AddJobsServices();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ErrorHandler>();
}
app.Run(async context =>
{
    var request = context.Request.Path.HasValue
        ? context.Request.Path.Value
        : string.Empty;

    await context.Response.WriteAsync($"EPA Auto Jobs. Request Path = {request}");
});

await app.RunAsync();