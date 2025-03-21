using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class PublicScheduleCommand : NonQueryCommand
{
    public int? EmployeeId { get; set; }

    public override string GetSettingKey() => "Auto_EmployeeCalendar_UpdateIsPublic";

    public override bool IsValid() => true;
}