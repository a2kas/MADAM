using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public interface IActualValueResolver
{
    string ResolveActualValue(string field, ItemBindingQualityCheckReferenceModel binding, ItemQualityCheckReferenceModel item);
}
