namespace HRM.Job.Models;

public class EmployeeSupervisor
{
    public int EmpId { get; set; }
    public int OwnerId { get; set; }

    public string Code { get; set; }
    public string FullName { get; set; }
    public string Nickname { get; set; }

    public string Email { get; set; }

    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
}