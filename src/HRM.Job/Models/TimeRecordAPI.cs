using System;
using System.Xml.Serialization;
using HRM.Job.Extensions;

namespace HRM.Job.Models;

[Serializable]
[XMLName("Row")]
public class TimeRecordAPI
{
    [XmlIgnore]
    public string Company { get; set; }

    public string EmpNo { get; set; }
    public byte? InOutType { get; set; }
    public DateTime? RecordTime { get; set; }
}