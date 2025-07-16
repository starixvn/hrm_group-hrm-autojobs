using System.Collections.Generic;
using Fanex.Data.Repository;
using Fanex.Logging;
using HRM.AutoJobs.Workers;
using HRM.Job.Models;
using HRM.Job.Services;
using HRM.Job.Services.Interfaces;
using HRM.LetStalk.Core.Services;
using HRM.LetStalk.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddSingleton<IPostnatalService, PostnatalService>();
        services.AddSingleton<IDisciplineService, DisciplineService>();
    }

    public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var letStalkSettings = configuration.GetSection("LetStalkSettings").Get<LetStalkSettings>();
        services.AddSingleton(letStalkSettings);

        var jobSettings = configuration.GetSection("JobSettings").Get<List<JobSetting>>();
        services.AddSingleton(jobSettings);

        var timeRecordApiSettings = configuration.GetSection("TimeRecordApiSettings").Get<List<TimeRecordApiSetting>>();
        services.AddSingleton(timeRecordApiSettings);
    }

    public static void AddJobsServices(this IServiceCollection services)
    {
        services.AddHostedService<SyncTimeRecordWorker>();
        services.AddHostedService<InsertHolidayInYearWorker>();
        services.AddHostedService<PublicScheduleWorker>();
        services.AddHostedService<InsertDefaultShiftWorker>();
        services.AddHostedService<InsertWeeklyDefaultShiftWorker>();
        services.AddHostedService<InsertALBalanceForNextYearWorker>();
        services.AddHostedService<InsertALBalanceForNewEmployeeWorker>();
        services.AddHostedService<RemindUsedALWorker>();
        services.AddHostedService<UpdateWorkingHourPerDayWorker>();
        services.AddHostedService<ProcessEmployeeAttendanceWorker>();
        services.AddHostedService<ProcessDisciplineWorker>();
    }
}