using MediatR;
using System.Text.RegularExpressions;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;

[RequiresPermission(Permissions.CanManageItemAssortmentSalesChannels)]
public class GetImportedAssortmentCommandHandler : IRequestHandler<GetImportedAssortmentCommand, Result<IEnumerable<ItemAssortmentItemModel>>>
{
    private readonly IItemBindingRepository _repository;

    public GetImportedAssortmentCommandHandler(IItemBindingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ItemAssortmentItemModel>>> Handle(GetImportedAssortmentCommand command, CancellationToken cancellationToken)
    {
        var itemNumbers = Regex.Split(command.Model.ItemNos, @"\s+|\r?\n").Where(x => !string.IsNullOrEmpty(x.Trim())).Distinct();
        var companies = Classifiers.Companies.Where(x => x.Country == command.Model.Country).Select(x => x.Value);

        var overview = await _repository.GetSalesChannelItemAssortmentOverview(itemNumbers, companies, cancellationToken);

        var result = new List<ItemAssortmentItemModel>();
        foreach (var itemNumber in itemNumbers)
        {
            var overviewItem = overview?.FirstOrDefault(x => x.ItemNo == itemNumber);
            result.Add(new ItemAssortmentItemModel()
            {
                ItemNo = itemNumber,
                ItemName = overviewItem?.ItemName,
                ItemBindingId = overviewItem?.ItemBindingId ?? 0,
            });
        }
        return Result<IEnumerable<ItemAssortmentItemModel>>.Success(result);
    }
}
