using Fanex.Logging;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using HRM.AutoJobs.Models;
using HRM.AutoJobs.Services;
using HRM.AutoJobs.Services.Interfaces;
using HRM.AutoJobs.Workers;
using HRM.LetStalk.Core.Services;
using HRM.LetStalk.Core.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using ILogger = Fanex.Logging.ILogger;

namespace HRM.AutoJobs;

public static class ServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IBaseService, BaseService>();
        services.AddSingleton<IBaseMailService, BaseMailService>();
        services.AddSingleton<IHttpClientService, HttpClientService>();
        services.AddSingleton<ISendNotificationService, SendNotificationService>();
        services.AddSingleton<ILetStalkService, LetStalkService>();
        services.AddSingleton<IScheduleArrangeService, ScheduleArrangeService>();
        services.AddSingleton<IAnnualLeaveBalanceService, AnnualLeaveBalanceService>();
        services.AddSingleton<IHolidayService, HolidayService>();
        services.AddSingleton<ITimeRecordService, TimeRecordService>();
        services.AddSingleton<ISyncWorkingShiftService, SyncWorkingShiftService>();
        services.AddSingleton<IPostnatalService, PostnatalService>();
        services.AddSingleton<IGroupReportService, GroupReportService>();
    }

    public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        services.AddSingleton(appSettings);

        var letStalkSettings = configuration.GetSection("LetStalkSettings").Get<LetStalkSettings>();
        services.AddSingleton(letStalkSettings);

        var jobSettings = configuration.GetSection("JobSettings").Get<List<JobSetting>>();
        services.AddSingleton(jobSettings);

        var userControlSettings = configuration.GetSection("UserControlSettings").Get<UserControlSettings>();
        services.AddSingleton(userControlSettings);
    }

    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisCache(configuration.GetSection("fanex.caching.redis"));

        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConfig);

        services.AddDataProtection().PersistKeysToStackExchangeRedis(redisMultiplexer, "HRM.Website:ProtectionKeys");

        services.AddSingleton(redisMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = "HRM.Website:Session:";
        });
    }

    public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
            options.Cookie.Name = "HRM.Website.Session";
        });

        return services;
    }

    public static void AddJobsServices(this IServiceCollection services)
    {
        services.AddHostedService<ImportTimeRecordWorker>();
        services.AddHostedService<InsertHolidayInYearWorker>();

        services.AddHostedService<PublicScheduleWorker>();
        services.AddHostedService<InsertDefaultShiftWorker>();
        services.AddHostedService<InsertWeeklyDefaultShiftWorker>();

        services.AddHostedService<InsertALBalanceForNextYearWorker>();
        services.AddHostedService<InsertALBalanceForNewEmployeeWorker>();
        services.AddHostedService<RemindUsedALWorker>();

        services.AddHostedService<SyncEmployeeInfoToGroupReportWorker>();
    }
}