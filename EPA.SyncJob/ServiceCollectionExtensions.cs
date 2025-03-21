using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Logging;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Logging;
using EPA.SyncJob.Workers;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using ILogger = Fanex.Logging.ILogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Services;
using EPA.SyncJob.Worker;

namespace EPA.SyncJob;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<ILogger, Logger>();

        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IBaseService, BaseService>();
        services.AddSingleton<IDepartmentService, DepartmentService>();


        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisCache(configuration.GetSection("fanex.caching.redis"));

        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConfig);

        services.AddDataProtection().PersistKeysToStackExchangeRedis(redisMultiplexer, "HRM.Website:ProtectionKeys");

        services.AddSingleton(redisMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = "EPA.sync.Website:Session:";
        });

        return services;
    }

    public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
            options.Cookie.Name = "EPA.Sync.Session";
        });

        return services;
    }

    public static IServiceCollection AddJobsServices(this IServiceCollection services)
    {
        services.AddHostedService<SyncDepartmentWorker>();
        return services;
    }
}