using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services;
using Microsoft.Extensions.Logging;

namespace HRM.AutoJobs.Workers;

public class ProcessDisciplineWorker : BaseWorker
{
    private readonly IDisciplineService disciplineService;

    public ProcessDisciplineWorker(
        IDisciplineService disciplineService,
        List<JobSetting> jobSettings,
        ILogger<ProcessDisciplineWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.disciplineService = disciplineService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.ProcessDiscipline);
    }

    public override async Task DoWorkAsync()
    {
        await disciplineService.ProcessDisciplineAsync();
    }
}