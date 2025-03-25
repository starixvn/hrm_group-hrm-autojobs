using System.Threading.Tasks;

namespace HRM.Job.Services.Interfaces;

public interface IAnnualLeaveBalanceService
{
    Task InsertForNextYearAsync(short companyId);

    Task InsertForNewEmployeeAsync(short companyId);

    Task RemindUsedALAsync(short companyId);
}