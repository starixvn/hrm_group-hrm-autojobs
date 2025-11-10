using Fanex.Data.Repository;

namespace HRM.Job.Criteria.AnnualLeave;

public class GetTWLeaveBalanceConfigsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "TWLeaveBalanceConfigs_Get";

    public override bool IsValid() => true;
}

