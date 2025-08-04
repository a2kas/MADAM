using System.ComponentModel;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

[DisplayName("New product offer")]
public class NewProductOffer : IMadamEntity<int>, IAuditable, IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public required int SupplierId { get; set; }

    public required int? ItemCategoryManagerId { get; set; }

    public required string FileReference { get; set; }

    public required NewProductOfferStatus Status { get; set; } = NewProductOfferStatus.New;

    public required BalticCountry Country { get; set; }

    public virtual required CategoryManager ItemCategoryManager { get; set; }
    public virtual required IEnumerable<NewProductOfferItem> Items { get; set; }
    public virtual required IEnumerable<NewProductOfferComment> Comments { get; set; }
    public virtual required IEnumerable<NewProductOfferAttachment> Attachments { get; set; }
}
