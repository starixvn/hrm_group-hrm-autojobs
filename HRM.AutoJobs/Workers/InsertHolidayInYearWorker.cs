using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InsertHolidayInYearWorker : BaseWorker
{
    private readonly IHolidayService holidayService;

    public InsertHolidayInYearWorker(
        IHolidayService holidayService,
        List<JobSetting> jobSettings,
        ILogger<InsertHolidayInYearWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.holidayService = holidayService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.InsertHolidayInYear);
    }

    public override async Task DoWorkAsync()
    {
        const byte September = 9;
        var today = DateTimeOffset.UtcNow;
        if (today is { Day: 1, Month: September })
        {
            await holidayService.InsertHolidayInYearAsync();
        }
    }
}