using System.Threading.Tasks;

namespace HRM.Job.Services.Interfaces;

public interface IScheduleArrangeService
{
    Task PublicScheduleAsync(short companyId);

    Task InsertDefaultShiftAsync(short companyId);

    Task InsertWeeklyDefaultShiftAsync(short companyId);
}