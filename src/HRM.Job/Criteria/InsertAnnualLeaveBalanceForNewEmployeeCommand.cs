using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class InsertAnnualLeaveBalanceForNewEmployeeCommand : NonQueryCommand
{
    public int Year { get; set; }
    public short CompanyId { get; set; }
    public override string GetSettingKey() => "Auto_AnnualLeaveBalance_UpdateForNewEmployee";

    public override bool IsValid() => Year > 0;
}