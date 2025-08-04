using System.ComponentModel;
using Tamro.Madam.Models.General;
using Tamro.Madam.RestApi.Contracts.SwaggerDoc;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.CategoryManagers.Index;
public class CategoryManagerViewModel
{
    [DisplayName("Id")]
    public int Id { get; set; }

    [DisplayName("Email address")]
    [DocumentAsFilterable]
    public string EmailAddress { get; set; }

    [DisplayName("First name")]
    [DocumentAsFilterable]
    public string FirstName { get; set; }

    [DisplayName("Last name")]
    [DocumentAsFilterable]
    public string LastName { get; set; }

    [DisplayName("Country")]
    public BalticCountry? Country { get; set; }
}
