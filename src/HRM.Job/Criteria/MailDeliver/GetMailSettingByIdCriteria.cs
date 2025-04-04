using Fanex.Data.Repository;

namespace HRM.Job.Criteria.MailDeliver;

public class GetMailSettingByIdCriteria : CriteriaBase
{
    public int SettingId { get; set; }

    public override string GetSettingKey() => "MailRecorder_GetSetting";

    public override bool IsValid() => SettingId > 0;
}