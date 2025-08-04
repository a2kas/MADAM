using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms.Grid;
public class SkuFormsGridQuery : SkuFormFilter, IRequest<PaginatedData<SkuFormModel>>, IDefaultErrorMessage
{
    public SkuFormSpecification Specification => new(this);

    public string ErrorMessage { get; set; } = "Failed to fetch data for SKU Forms grid";
}
