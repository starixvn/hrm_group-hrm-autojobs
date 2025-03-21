using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class ImportTimeRecordCommand : NonQueryCommand
{
    public override string GetSettingKey() => "TimeRecord_ImportData";

    public override bool IsValid() => true;
}