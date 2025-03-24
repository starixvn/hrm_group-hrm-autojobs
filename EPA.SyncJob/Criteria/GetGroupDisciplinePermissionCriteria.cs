namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetGroupDisciplinePermissionCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public short CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_GroupSupervisor_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}