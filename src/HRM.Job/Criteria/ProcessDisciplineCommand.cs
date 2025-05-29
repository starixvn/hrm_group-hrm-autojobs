using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class ProcessDisciplineCommand : NonQueryCommand
{
    public override string GetSettingKey() => "Auto_Discipline_ProcessWaitingCase";

    public override bool IsValid() => true;
}