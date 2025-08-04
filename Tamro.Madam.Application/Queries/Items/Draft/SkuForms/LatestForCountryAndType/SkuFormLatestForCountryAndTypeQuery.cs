using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountryAndType;
public class SkuFormLatestForCountryAndTypeQuery : IRequest<Result<FileWithName?>>, IDefaultErrorMessage
{
    public BalticCountry Country { get; set; }
    public SkuFormType Type { get; set; }

    public string ErrorMessage { get => $"Failed to fetch the last SKU Form for {Country}"; set { } }
}
