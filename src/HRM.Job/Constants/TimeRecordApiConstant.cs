using System.Collections.Generic;
using System.Collections.Immutable;

namespace HRM.Job.Constants;

public static class TimeRecordApiConstant
{
    public static readonly ImmutableDictionary<string, short> CompanyDict =
        new Dictionary<string, short>
        {
            {"SMS", 3},
            {"HM", 4},
            {"LEX", 6}
        }.ToImmutableDictionary();
}