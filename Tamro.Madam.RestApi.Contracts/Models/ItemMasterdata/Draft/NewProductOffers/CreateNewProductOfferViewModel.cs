using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.NewProductOffers;

public class CreateNewProductOfferViewModel
{
    [DisplayName("Supplier id")]
    [Range(1, int.MaxValue)]
    public required int SupplierId { get; set; }

    [DisplayName("Category manager id")]
    [Range(1, int.MaxValue)]
    public int? ItemCategoryManagerId { get; set; }

    [DisplayName("Country")]
    public required BalticCountry Country { get; set; }

    public required IFormFile File { get; set; }
}
