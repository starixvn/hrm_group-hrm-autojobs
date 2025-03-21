using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class UpdateAnnualLeaveBalanceCommand : NonQueryCommand
{
    public int Year { get; set; }
    public int? EmpId { get; set; }

    public override string GetSettingKey() => "Auto_AnnualLeaveBalance_Update";

    public override bool IsValid() => Year > 0;
}