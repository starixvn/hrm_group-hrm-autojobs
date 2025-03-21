namespace HRM.Job.Services.Interfaces;

public interface IScheduleArrangeService
{
    Task PublicScheduleAsync();

    Task InsertDefaultShiftAsync();

    Task InsertWeeklyDefaultShiftAsync();
}