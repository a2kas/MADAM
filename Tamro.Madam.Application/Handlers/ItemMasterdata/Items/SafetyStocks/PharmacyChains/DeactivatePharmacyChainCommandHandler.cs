using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

[RequiresPermission(Permissions.CanManageSafetyStock)]
public class DeactivatePharmacyChainCommandHandler : IRequestHandler<DeactivatePharmacyChainCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public DeactivatePharmacyChainCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeactivatePharmacyChainCommand command, CancellationToken cancellationToken)
    {
        var pharmacyChainsToUpdate = await _uow.GetRepository<SafetyStockPharmacyChain>()
            .AsQueryable()
            .Where(x => command.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var pharmacyChain in pharmacyChainsToUpdate)
        {
            pharmacyChain.IsActive = false;
        }

        var result = await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(result);
    }
}
