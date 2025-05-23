using Fanex.Data.Repository;

namespace HRM.Job.Criteria.AnnualLeave;

public class UpdateAnnualLeaveBalanceByJoinedDateCriteria : NonQueryCommand
{
    public string ListAnnualLeaveJson { get; set; }

    public override string GetSettingKey() => "Auto_AnnualLeaveBalance_JoinedDate_Update";

    public override bool IsValid() => true;
}