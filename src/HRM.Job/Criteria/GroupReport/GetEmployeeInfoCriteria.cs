using Fanex.Data.Repository;

namespace HRM.Job.Criteria.GroupReport;

public class GetEmployeeInfoCriteria : CriteriaBase
{
    public DateTime? LastModifiedDate { get; set; }

    public override string GetSettingKey() => "HRM_SyncData_GroupReport_GetEmployeeInfo";

    public override bool IsValid() => true;
}