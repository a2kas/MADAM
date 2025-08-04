using System;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.NewProductOffers;

public class CreateNewProductOfferResultViewModel
{
    public required int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public required int SupplierId { get; set; }
    public required int ItemCategoryManagerId { get; set; }
    public required NewProductOfferStatus Status { get; set; }
    public required BalticCountry Country { get; set; }
    public required string FileReference { get; set; }
}
