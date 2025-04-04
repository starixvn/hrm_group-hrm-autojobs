using System;
using Fanex.Data.Repository;

namespace HRM.Job.Criteria;

public class ImportTimeRecordCommand : NonQueryCommand
{
    public short CompanyId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string TimeRecordXml { get; set; }

    public override string GetSettingKey() => "Auto_TimeRecord_Import";

    public override bool IsValid() => true;
}