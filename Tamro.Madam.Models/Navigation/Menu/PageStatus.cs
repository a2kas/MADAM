using System.ComponentModel;

namespace Tamro.Madam.Models.Navigation.Menu;

public enum PageStatus
{
    [Description("Coming Soon")]
    ComingSoon,
    [Description("WIP")]
    Wip,
    [Description("New")]
    New,
    [Description("Completed")]
    Completed
}
