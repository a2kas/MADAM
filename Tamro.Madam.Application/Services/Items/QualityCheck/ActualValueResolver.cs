using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public class ActualValueResolver : IActualValueResolver
{
    public string ResolveActualValue(string field, ItemBindingQualityCheckReferenceModel binding, ItemQualityCheckReferenceModel item)
    {
        return field switch
        {
            "strength" => item.Strength,
            "activeIngredient" => item.Atc,
            "name" => item.ItemName,
            "shortDescription" => binding.ShortDescription,
            "fullDescription" => binding.Description,
            "usage" => binding.Usage,
            _ => string.Empty,
        };
    }
}
