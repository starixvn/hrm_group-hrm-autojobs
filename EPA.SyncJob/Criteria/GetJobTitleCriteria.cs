namespace EPA.SyncJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetJobTitleCriteria : CriteriaBase
    {
        public short CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_JobTitle_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}