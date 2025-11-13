using Fanex.Data.Repository;

namespace HRM.Job.Criteria.AnnualLeave;

public class UpdateAnnualLeaveBalanceByYearlyCriteria : NonQueryCommand
{
    public string ListAnnualLeaveJson { get; set; }

    public int Year { get; set; }

    public override string GetSettingKey() => "Auto_AnnualLeaveBalance_Yearly_Update";

    public override bool IsValid() => true;
}