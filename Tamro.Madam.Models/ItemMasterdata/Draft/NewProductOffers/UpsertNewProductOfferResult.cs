using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
public class UpsertNewProductOfferResult
{
    public required int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public required int SupplierId { get; set; }
    public required int ItemCategoryManagerId { get; set; }
    public required NewProductOfferStatus Status { get; set; }
    public required BalticCountry Country { get; set; }
    public required string FileReference { get; set; }
}
