namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetTeamCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }
        public short CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_EmployeeGroup_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}