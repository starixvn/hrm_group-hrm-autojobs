namespace EPA.SyncJob.Models;

using System;

[Serializable]
public class CommandResult
{
    public int Code { get; set; } = 0;
    public string Message { get; set; } = "";
    public bool IsSuccessful => Code == 0;
}