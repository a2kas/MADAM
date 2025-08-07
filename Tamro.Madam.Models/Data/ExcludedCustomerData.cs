using System.Collections.Immutable;

namespace Tamro.Madam.Models.Data;
public static class ExcludedCustomerData
{
    public static readonly ImmutableDictionary<string, string> ExclusionLevels = new Dictionary<string, string>
    {
        { nameof(ExclusionLevel.EntireLegalEntity), "Entire legal entity" },
        { nameof(ExclusionLevel.OneOrMorePhysicalLocations), "One or more physical locations" }
    }.ToImmutableDictionary();
}
