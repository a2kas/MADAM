using System.ComponentModel;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;

namespace Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
internal class NewProductOfferModel
{
    [DisplayName("Id")]
    public int Id { get; set; }

    [DisplayName("Supplier id")]
    public required int SupplierId { get; set; }

    [DisplayName("Category manager id")]
    public required int? ItemCategoryManagerId { get; set; }

    [DisplayName("File reference")]
    public required string FileReference { get; set; }

    [DisplayName("Status")]
    public required NewProductOfferStatus Status { get; set; } = NewProductOfferStatus.New;

    [DisplayName("Country")]
    public required BalticCountry Country { get; set; }

    [DisplayName("Category manager")]
    public virtual required CategoryManagerModel ItemCategoryManager { get; set; }
}
