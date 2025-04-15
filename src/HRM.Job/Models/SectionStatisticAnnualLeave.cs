namespace HRM.Job.Models;

public class SectionStatisticAnnualLeave
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }

    public int TotalEmployee { get; set; }

    public int AnnualLeaveBalance { get; set; }

    public double Percentage { get; set; }
}