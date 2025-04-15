namespace HRM.Job.Models;

public class AnnualLeaveBalance
{
    public int EmpId { get; set; }
    public string Code { get; set; }
    public string Nickname { get; set; }
    public string FullName { get; set; }

    public string Email { get; set; } = string.Empty;
    public bool IsLocal { get; set; }

    public float AnnualLeaveCurrentYear { get; set; }

    public float LeaveBalance { get; set; }
    public string LetStalk { get; set; }

    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
}