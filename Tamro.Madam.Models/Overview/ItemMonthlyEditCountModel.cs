using System.Globalization;

namespace Tamro.Madam.Models.Overview;

public class ItemMonthlyEditCountModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Count { get; set; }
    public string Label
    {
        get
        {
            return DateTime.ParseExact($"{Month}.{Year}", "M.yyyy", CultureInfo.InvariantCulture).ToString("MMM.yy", CultureInfo.InvariantCulture);
        }
    }
}
