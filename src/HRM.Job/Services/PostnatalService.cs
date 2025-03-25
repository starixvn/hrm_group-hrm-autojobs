using System.Threading.Tasks;
using HRM.Job.Criteria.Postnatal;
using HRM.Job.Services.Interfaces;

namespace HRM.Job.Services;

public class PostnatalService(IBaseService baseService) : IPostnatalService
{
    public async Task UpdateWorkingHourPerDayAsync()
    {
        var criteria = new UpdateWorkingHourPerDayCommand();
        await baseService.ExecuteNonQueryAsync(criteria);
    }
}