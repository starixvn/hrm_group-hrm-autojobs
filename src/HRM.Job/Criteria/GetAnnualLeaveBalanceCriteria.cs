using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class GetAnnualLeaveBalanceCriteria : CriteriaBase
{
    public int Year { get; set; }

    public override string GetSettingKey() => "Auto_AnnualLeaveBalance_Get";

    public override bool IsValid() => true;
}