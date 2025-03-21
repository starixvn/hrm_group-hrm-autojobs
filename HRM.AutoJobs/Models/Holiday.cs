using HRM.AutoJobs.Extensions;

namespace HRM.AutoJobs.Models;

using System;

[Serializable]
[XMLName("Row")]
public class Holiday
{
    public string HolidayName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}