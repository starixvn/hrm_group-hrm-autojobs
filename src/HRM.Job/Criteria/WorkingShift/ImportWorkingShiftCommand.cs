using Fanex.Data.Repository;

namespace HRM.Job.Criteria.WorkingShift;

public class ImportWorkingShiftCommand : NonQueryCommand
{
    public string WorkingShiftXml { get; set; }

    public override string GetSettingKey() => "Sync_WorkingShift_Import";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(WorkingShiftXml);
}