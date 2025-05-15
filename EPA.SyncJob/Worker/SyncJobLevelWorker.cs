using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;

namespace EPA.SyncJob.Worker;

public class SyncJobLevelWorker : BaseWorker
{
    private readonly IJobLevelService jobLevelService;
    private readonly ITokenService tokenService;
    private readonly List<ServiceAPI> serviceApis;
    private readonly ILogger<SyncJobLevelWorker> logfile;
    private readonly Fanex.Logging.ILogger logger;

    public SyncJobLevelWorker(
        IJobLevelService jobLevelService,
        ITokenService tokenService,
        List<JobSetting> jobSettings,
        List<ServiceAPI> serviceApis,
        ILogger<SyncJobLevelWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncJobLevel);
        this.jobLevelService = jobLevelService;
        this.tokenService = tokenService;
        this.serviceApis = serviceApis;
        this.logfile = logfile;
        this.logger = logger;
    }

    public override async Task DoWorkAsync()
    {
        var serviceApiList = serviceApis.ToList();
        var tasks = serviceApiList.Select(async serviceApi =>
        {
            try
            {
                await UpdateAsync(serviceApi);
                logfile.LogInformation($"Successful SyncJobLevel service API: {serviceApi.Name}");
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync($"Error SyncJobLevel service API: {serviceApi.Name} - {ex}");
                logfile.LogError($"Error SyncJobLevel service API: {serviceApi.Name} - {ex}");
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task UpdateAsync(ServiceAPI serviceApi)
    {
        var token = await tokenService.Request(serviceApi);
        if (!token.IsValid())
        {
            throw new InvalidOperationException("Invalid token");
        }
        var fromDate = await jobLevelService.GetLastModifiedDateAsync(token, serviceApi);
        var data = await jobLevelService.GetJobLevelAsync(fromDate);
        if (data.Any())
        {
            await jobLevelService.UpdateAsync(data, token, serviceApi);
        }
    }
}