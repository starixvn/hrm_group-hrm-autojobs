using HRM.Job.Criteria;
using HRM.Job.Services.Interfaces;

namespace HRM.Job.Services;

public class TimeRecordService(IBaseService baseService) : ITimeRecordService
{
    public async Task ImportTimeRecordAsync()
    {
        var command = new ImportTimeRecordCommand();
        await baseService.ExecuteNonQueryAsync(command);
    }
}