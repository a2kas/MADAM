using System.ComponentModel;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOfferComment;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

[DisplayName("New product offer comment")]
public class NewProductOfferComment : IMadamEntity<int>, IAuditable, IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public required int NewProductOfferId { get; set; }
    public required string Comment { get; set; }
    public required string Author { get; set; }
    public required NewProductOfferComentSubmittedBy SubmittedBy { get; set; }

    public virtual required NewProductOffer NewProductOffer { get; set; }
}
