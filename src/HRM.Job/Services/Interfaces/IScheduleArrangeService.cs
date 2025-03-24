namespace HRM.Job.Services.Interfaces;

public interface IScheduleArrangeService
{
    Task PublicScheduleAsync(List<short> companyIds);

    Task InsertDefaultShiftAsync();

    Task InsertWeeklyDefaultShiftAsync();
}