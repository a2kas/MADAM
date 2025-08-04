using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;

[DisplayName("Item category manager")]
public class CategoryManager : IMadamEntity<int>, IAuditable, IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    [DisplayName("Email address")]
    public required string EmailAddress { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required BalticCountry Country { get; set; }

    public virtual required ICollection<NewProductOffer> NewProductOffers { get; set; }
}
