using EPA.SyncJob.Models;
using EPA.SyncJob.Services;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.ThirdPartyAPI;
using EPA.SyncJob.Worker;
using Fanex.Data.Repository;
using Fanex.Logging;
using ILogger = Fanex.Logging.ILogger;

namespace EPA.SyncJob;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serviveAPIs = configuration.GetSection("ServiceAPIs").Get<List<ServiceAPI>>();
        services.AddSingleton(serviveAPIs);

        var jobSettings = configuration.GetSection("JobSettings").Get<List<JobSetting>>();
        services.AddSingleton(jobSettings);

        services.AddHttpClient();

        services.AddSingleton<ILogger, Logger>();

        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IBaseService, BaseService>();
        services.AddSingleton<IDepartmentService, DepartmentService>();
        services.AddSingleton<IDepartmentAPI, DepartmentAPI>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<ITokenAPI, TokenAPI>();
        services.AddSingleton<ICompanyService, CompanyService>();
        services.AddSingleton<ITeamService, TeamService>();
        services.AddSingleton<ITeamAPI, TeamAPI>();
        services.AddSingleton<IEmployeeService, EmployeeService>();
        services.AddSingleton<IEmployeeAPI, EmployeeAPI>();
        services.AddSingleton<IJobTitleService, JobTitleService>();
        services.AddSingleton<IJobTitleAPI, JobTitleAPI>();
        services.AddSingleton<IDisciplinePermissionService, DisciplinePermissionService>();
        services.AddSingleton<IEmployeeDisciplinePermissionAPI, EmployeeDisciplinePermissionAPI>();
        services.AddSingleton<ITeamDisciplinePermissionAPI, TeamDisciplinePermissionAPI>();
        services.AddSingleton<IDisciplineSummaryAPI, DisciplineSummaryAPI>();
        services.AddSingleton<IDisciplineSummaryService, DisciplineSummaryService>();
        services.AddSingleton<IJobLevelService, JobLevelService>();
        services.AddSingleton<IJobLevelAPI, JobLevelAPI>();

        services.AddSingleton<IHttpClientService, HttpClientService>();

        return services;
    }

    public static IServiceCollection AddJobsServices(this IServiceCollection services)
    {
        services.AddHostedService<SyncDepartmentWorker>();
        services.AddHostedService<SyncTeamWorker>();
        services.AddHostedService<SyncEmployeeWorker>();
        services.AddHostedService<SyncJobTitleWorker>();
        services.AddHostedService<SyncEmployeeDisciplinePermissionWorker>();
        services.AddHostedService<SyncGroupDisciplinePermissionWorker>();
        services.AddHostedService<SyncDisciplineSummaryWorker>();
        services.AddHostedService<SyncJobLevelWorker>();

        return services;
    }
}