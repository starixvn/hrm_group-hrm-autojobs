using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Job.Services.Interfaces;

public interface ITimeRecordService
{
    Task ImportTimeRecordAsync();

    Task ProcessEmployeeAttendanceAsync(List<short> companyIds);
}