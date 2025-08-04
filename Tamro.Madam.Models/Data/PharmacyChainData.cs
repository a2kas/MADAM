using System.Collections.Immutable;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Data;

public static class PharmacyChainData
{
    public static ImmutableDictionary<string, string> GroupDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
           .Add(PharmacyGroup.All.ToString(), PharmacyGroup.All.ToString())
           .Add(PharmacyGroup.Benu.ToString(), PharmacyGroup.Benu.ToString())
           .Add(PharmacyGroup.NonBenu.ToString(), PharmacyGroup.NonBenu.ToString());
}
