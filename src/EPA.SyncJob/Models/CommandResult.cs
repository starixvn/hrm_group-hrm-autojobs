namespace EPA.SyncJob.Models;

using System;

[Serializable]
public class CommandResult
{
    public int Code { get; set; } = 1;
    public string Message { get; set; } = "Invalid data.";
}