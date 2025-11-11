using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Job.Models.AnnualLeave;

namespace HRM.Job.Services.Interfaces;

public interface IAnnualLeaveBalanceService
{
    Task InsertForNextYearByYearlyAsync(AnnualLeaveResetConfig config);

    Task InsertForNewEmployeesByYearlyAsync(AnnualLeaveResetConfig config);

    Task RemindUsedALAsync(short companyId);

    Task<IEnumerable<AnnualLeaveResetConfig>> GetAnnualLeaveResetConfigs();

    Task InsertForNextYearByJoinedDateAsync(AnnualLeaveResetConfig config);

    Task InsertForNewEmployeesByJoinedDateAsync(AnnualLeaveResetConfig config);
}