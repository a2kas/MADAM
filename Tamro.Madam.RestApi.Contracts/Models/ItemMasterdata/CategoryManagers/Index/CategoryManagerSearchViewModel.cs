using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.CategoryManagers.Index;
public class CategoryManagerSearchViewModel : PaginationFilter
{
    [FromQuery]
    public ICollection<FilterDefinition> Filters { get; set; } = new List<FilterDefinition>();
    public string? SearchTerm { get; set; }
    public HashSet<BalticCountry>? Countries { get; set; }
}
