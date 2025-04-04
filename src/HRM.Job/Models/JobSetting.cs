using System;
using System.Collections.Generic;

namespace HRM.Job.Models;

[Serializable]
public class JobSetting
{
    public string JobName { get; set; }
    public string CronExpression { get; set; }
    public bool Enabled { get; set; }
    public List<short> CompanyIds { get; set; } = [];
}