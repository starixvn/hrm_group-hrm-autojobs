namespace EPA.SyncJob.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetDisciplineSummaryCriteria : CriteriaBase
    {
        public DateTime LastModifiedDate { get; set; }

        public short CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "SyncEPA_DisciplineSummary";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}