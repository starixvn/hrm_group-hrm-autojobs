using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Job.Criteria;
using HRM.Job.Services.Interfaces;

namespace HRM.Job.Services;

public class ScheduleArrangeService(IBaseService baseService) : IScheduleArrangeService
{
    public async Task PublicScheduleAsync(short companyId)
    {
        var command = new PublicScheduleCommand { CompanyId = companyId };
        await baseService.ExecuteNonQueryAsync(command);
    }

    public async Task InsertDefaultShiftAsync(short companyId)
    {
        var fromDate = LastDayOfWeek(DateTimeOffset.Now.Date).AddDays(1);
        var toDate = fromDate.AddDays(6);
        var command = new InsertScheduleByDefaultShiftCommand
        {
            FromDate = fromDate,
            ToDate = toDate,
            CompanyId = companyId
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    public async Task InsertWeeklyDefaultShiftAsync(short companyId)
    {
        var fromDate = LastDayOfWeek(DateTimeOffset.Now.Date).AddDays(1);
        var toDate = fromDate.AddDays(6);
        var command = new InsertScheduleByWeeklyDefaultShiftCommand
        {
            FromDate = fromDate,
            ToDate = toDate,
            CompanyId = companyId
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    private static DateTime FirstDayOfWeek(DateTime dt)
    {
        var diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    private static DateTime LastDayOfWeek(DateTime dt)
    {
        const byte AddDays = 6;
        return FirstDayOfWeek(dt).AddDays(AddDays);
    }
}