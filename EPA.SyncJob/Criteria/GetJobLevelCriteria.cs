namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetJobLevelCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_JobLevel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}