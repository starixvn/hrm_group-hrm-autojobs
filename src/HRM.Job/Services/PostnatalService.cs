using System.Threading.Tasks;
using HRM.Job.Criteria.Postnatal;
using HRM.Job.Services.Interfaces;

namespace HRM.Job.Services;

public class PostnatalService(IBaseService baseService) : IPostnatalService
{
    public async Task UpdateWorkingHourPerDayAsync(short companyId)
    {
        var criteria = new UpdateWorkingHourPerDayCommand { CompanyId = companyId };
        await baseService.ExecuteNonQueryAsync(criteria);
    }
}