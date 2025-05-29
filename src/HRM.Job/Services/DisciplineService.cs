using System.Threading.Tasks;
using HRM.Job.Criteria;
using HRM.Job.Services.Interfaces;

namespace HRM.Job.Services;

public interface IDisciplineService
{
    Task ProcessDisciplineAsync();
}

public class DisciplineService(IBaseService baseService) : IDisciplineService
{
    public async Task ProcessDisciplineAsync()
    {
        var command = new ProcessDisciplineCommand();
        await baseService.ExecuteNonQueryAsync(command);
    }
}