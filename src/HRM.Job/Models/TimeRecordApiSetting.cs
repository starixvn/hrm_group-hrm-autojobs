using System;

namespace HRM.Job.Models;

[Serializable]
public class TimeRecordApiSetting
{
    public string Url { get; set; }
    public string ApiKey { get; set; }
    public float Timezone { get; set; }
}