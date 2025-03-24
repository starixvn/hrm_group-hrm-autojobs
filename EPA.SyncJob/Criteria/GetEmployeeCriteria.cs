namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }
        public short CompanyId { get; set; }

        public override string GetSettingKey() => "SyncEPA_Employee_Get";

        public override bool IsValid() => true;
    }
}