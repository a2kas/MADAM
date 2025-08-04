using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.QualityCheck;

public class ItemQualityCheckQuery : ItemQualityCheckFilter, IRequest<PaginatedData<ItemQualityCheckGridModel>>
{
    public ItemQualityCheckSpecification Specification => new(this);
}
