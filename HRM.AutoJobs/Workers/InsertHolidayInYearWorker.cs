namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
        const byte August = 8;
        var today = DateTimeOffset.UtcNow;
        var lastDay = DateTime.DaysInMonth(today.Year, today.Month);

        if (today.Day == lastDay && today.Month == August)
        {
            foreach (var companyId in jobSetting.CompanyIds)
            {
                await holidayService.InsertHolidayInYearAsync(companyId);
            }
        }
    }
}