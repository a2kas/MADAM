using System.Collections.Immutable;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Hr.Jira.Administration;

namespace Tamro.Madam.Models.Data;

public class GenericData
{
    public static ImmutableDictionary<string, string> CountryDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
       .Add(LanguageCountry.EE.ToString(), LanguageCountry.EE.ToString())
       .Add(LanguageCountry.LT.ToString(), LanguageCountry.LT.ToString())
       .Add(LanguageCountry.LV.ToString(), LanguageCountry.LV.ToString());

    public static ImmutableDictionary<string, string> TeamDisplayName { get; } = ImmutableDictionary<string, string>.Empty
        .Add("[No value]", "null")
        .Add(JiraAccountTeam.E1.ToString(), JiraAccountTeam.E1.ToString())
        .Add(JiraAccountTeam.Dev.ToString(), JiraAccountTeam.Dev.ToString())
        .Add(JiraAccountTeam.Online.ToString(), JiraAccountTeam.Online.ToString())
        .Add(JiraAccountTeam.BI.ToString(), JiraAccountTeam.BI.ToString())
        .Add(JiraAccountTeam.BalSupport.ToString(), JiraAccountTeam.BalSupport.ToString())
        .Add(JiraAccountTeam.HansaSoft.ToString(), JiraAccountTeam.HansaSoft.ToString())
        .Add(JiraAccountTeam.PMO.ToString(), JiraAccountTeam.PMO.ToString())
        .Add(JiraAccountTeam.Business.ToString(), JiraAccountTeam.Business.ToString())
        .Add(JiraAccountTeam.External.ToString(), JiraAccountTeam.External.ToString());

    public static ImmutableDictionary<string, string> BooleanDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
        .Add("Yes", "true")
        .Add("No", "false");
}
