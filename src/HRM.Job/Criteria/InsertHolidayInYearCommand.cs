using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class InsertHolidayInYearCommand : NonQueryCommand
{
    public string Holidays { get; set; }

    public override string GetSettingKey() => "Holiday_AutoGenerate";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Holidays);
}