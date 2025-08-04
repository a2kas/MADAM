using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public interface IQualityCheckAiConsumerService
{
    string ConstructPrompt();
    Task<ItemQualityCheckReferenceModel> ConstructReference(Item item);
    string ConstructSchema();
}
