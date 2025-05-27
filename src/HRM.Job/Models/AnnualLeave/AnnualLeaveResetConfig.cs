using HRM.Job.Enum;

namespace HRM.Job.Models.AnnualLeave;

public class AnnualLeaveResetConfig
{
    public short CompanyId { get; set; }

    public AnnualLeaveTimeline Timeline { get; set; }

    private string EnableAdditionalAnnualLeaveBalance { get; set; }

    public bool IsEnableAdditionalAnnualLeaveBalance
    {
        get => EnableAdditionalAnnualLeaveBalance == "1";
        set => EnableAdditionalAnnualLeaveBalance = value ? "1" : "0";
    }
}