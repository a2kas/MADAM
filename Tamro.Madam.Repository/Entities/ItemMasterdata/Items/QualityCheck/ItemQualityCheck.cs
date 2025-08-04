using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;
public class ItemQualityCheck : IMadamEntity<int>, IBaseEntity
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public ICollection<ItemQualityCheckIssue> ItemQualityCheckIssues { get; set; }
    public Item Item { get; set; }
}
