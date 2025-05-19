using Fanex.Data.Repository;

namespace HRM.Job.Criteria.AnnualLeave;

public class GetEmployeesServiceDetailCriteria : CriteriaBase
{
    public short CompanyId { get; set; }

    public bool IsNewEmployee { get; set; }

    public override string GetSettingKey() => "Employees_ServiceDetail_Get";
    
    public override bool IsValid()
    {
        return true;
    }
}           