using Fanex.Data.Repository;

namespace HRM.Job.Criteria.AnnualLeave;

public class GetAnnualLeaveResetConfigsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "AnnualLeave_ResetConfigs_Get";

    public override bool IsValid() => true;
}