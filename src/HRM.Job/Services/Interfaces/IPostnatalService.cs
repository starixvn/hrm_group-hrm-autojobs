using System.Threading.Tasks;

namespace HRM.Job.Services.Interfaces;

public interface IPostnatalService
{
    Task UpdateWorkingHourPerDayAsync(short companyId);
}