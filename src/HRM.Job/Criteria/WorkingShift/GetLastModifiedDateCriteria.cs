using Fanex.Data.Repository;

namespace HRM.Job.Criteria.WorkingShift;

public class GetLastModifiedDateCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Sync_WorkingShift_GetLastModifiedDate";

    public override bool IsValid() => true;
}