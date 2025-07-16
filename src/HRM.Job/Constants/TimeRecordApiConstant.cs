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
            {"LEX", 6},
            {"CTS", 9},
            {"AT", 11}, // Ascentis
            {"Hyjay", 18},
            {"NF", 20}, // Next Fortune
            {"OL", 22}, // One Lab
            {"EMC", 31},
            {"SSI", 33}
        }.ToImmutableDictionary();
}