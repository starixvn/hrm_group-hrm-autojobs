using HRM.Job.Extensions;

namespace HRM.Job.Models;

[Serializable]
[XMLName("Row")]
public class Holiday
{
    public string HolidayName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}