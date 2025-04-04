namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeDisciplinePermissionCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }
        public short CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_EmployeeSupervisor_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}