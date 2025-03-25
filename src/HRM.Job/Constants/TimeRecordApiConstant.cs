using System.Collections.Generic;
using System.Collections.Immutable;

namespace HRM.Job.Constants;

public static class TimeRecordApiConstant
{
    public static readonly ImmutableDictionary<string, (short, short)> CompanyDict =
        new Dictionary<string, (short, short)>
        {
            {"SMS", (3, 7)}
        }.ToImmutableDictionary();
}