using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Commerce.Assortment;

public class ItemAssortmentSalesChannel : IMadamEntity<int>, IBaseEntity, IAuditable
{
    [Key]
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public ICollection<ItemAssortmentBindingMap> ItemAssortmentBindingMaps { get; set; }
}
