using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;

public class ItemAssortmentBindingMap : IMadamEntity<int>, IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public int ItemBindingId { get; set; }
    public int ItemAssortmentSalesChannelId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public ItemBinding ItemBinding { get; set; }
    public ItemAssortmentSalesChannel ItemAssortmentSalesChannel { get; set; }
}
