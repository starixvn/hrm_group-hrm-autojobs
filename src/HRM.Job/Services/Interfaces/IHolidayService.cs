using System.Threading.Tasks;

namespace HRM.Job.Services.Interfaces;

public interface IHolidayService
{
    Task InsertHolidayInYearAsync(short companyId);
}