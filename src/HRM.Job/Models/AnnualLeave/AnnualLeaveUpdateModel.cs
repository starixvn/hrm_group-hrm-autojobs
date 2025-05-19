namespace HRM.Job.Models.AnnualLeave
{
    public class AnnualLeaveUpdateModel
    {
        public int EmpId { get; set; }

        public int AnnualLeaves { get; set; }

        public int ExtraLeaves { get; set; }

        public int Year { get; set; }
    }
}
