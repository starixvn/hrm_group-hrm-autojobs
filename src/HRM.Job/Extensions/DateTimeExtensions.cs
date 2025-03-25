using System;

namespace HRM.Job.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeSpan ServerTimeZone = TimeSpan.FromHours(-4);

    public static DateTime ToServerTimeZone(this DateTime time, short clientTimeZone)
    {
        var timeZone = TimeSpan.FromHours(clientTimeZone);
        var offsetTime = new DateTimeOffset(time, timeZone);
        return offsetTime.ToOffset(ServerTimeZone).DateTime;
    }
}