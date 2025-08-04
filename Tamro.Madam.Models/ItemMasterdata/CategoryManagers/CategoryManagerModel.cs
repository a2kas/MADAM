using System.ComponentModel;
using Tamro.Madam.Models.Common;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
public class CategoryManagerModel : BaseDataGridModel<CategoryManagerModel>
{
    [DisplayName("Id")]
    public int Id { get; set; }

    [DisplayName("Email address")]
    public string EmailAddress { get; set; }

    [DisplayName("First name")]
    public string FirstName { get; set; }

    [DisplayName("Last name")]
    public string LastName { get; set; }

    [DisplayName("Country")]
    public BalticCountry? Country { get; set; }

    public DateTime? RowVer { get; set; }
}
