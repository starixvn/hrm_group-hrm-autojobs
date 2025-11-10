using System;

namespace HRM.Job.Models.AnnualLeave;

public class EmployeeServiceDetail
{
    public int EmpId { get; set; }

    public int AnnualLeave { get; set; }

    public DateTime JoinedDate { get; set; }

    public int WorkingDays { get; set; }

    public int ExtraLeaveForYearsOfService { get; set; }

    public DateTime? MilitaryFromDate { get; set; }

    public DateTime? MilitaryToDate { get; set; }
}