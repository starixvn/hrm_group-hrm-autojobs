using Fanex.Data.Repository;

namespace HRM.Job.Criteria.Postnatal;

public class UpdateWorkingHourPerDayCommand : NonQueryCommand
{
    public short CompanyId { get; set; }

    public override string GetSettingKey() => "Auto_Postnatal_UpdateWorkingHour";

    public override bool IsValid() => true;
}