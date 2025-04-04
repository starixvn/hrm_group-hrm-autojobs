namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetDepartmentCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }
        public short CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_Department";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}