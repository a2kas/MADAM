using System.ComponentModel;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

[DisplayName("New product offer attachment")]
public class NewProductOfferAttachment : IMadamEntity<int>, IAuditable, IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public required int NewProductOfferId { get; set; }
    public required string FileReference { get; set; }

    public virtual required NewProductOffer NewProductOffer { get; set; }
}
