using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Brands;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Brands;

public class GetBrandsDeletionOverviewCommand : IRequest<Result<IEnumerable<BrandDeletionOverviewModel>>>, IDefaultErrorMessage
{
    public GetBrandsDeletionOverviewCommand(List<BrandModel> brands)
    {
        Brands = brands;
    }

    public List<BrandModel> Brands { get; }
    public string ErrorMessage { get; set; } = "Failed to retrieve brand deletion issues";
}
