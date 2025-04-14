using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class ProcessEmployeeAttendanceCommand : NonQueryCommand
{
    public short CompanyId { get; set; }

    public override string GetSettingKey() => "EmployeeAttendance_ForWaitingCase";

    public override bool IsValid() => true;
}