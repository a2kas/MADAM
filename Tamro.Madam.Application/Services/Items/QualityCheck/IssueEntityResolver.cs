using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public class IssueEntityResolver : IIssueEntityResolver
{
    public string ResolveIssueEntity(string field)
    {
        return field switch
        {
            "strength" or "activeIngredient" or "name" => nameof(Item),
            _ => nameof(ItemBinding),
        };
    }
}
