using System.Threading.Tasks;

namespace HRM.Job.Services.Interfaces;

public interface IAnnualLeaveBalanceService
{
    Task InsertForNextYearAsync();

    Task InsertForNewEmployeeAsync();

    Task RemindUsedALAsync();
}