using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.Sabis;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Context.E1Gateway;

namespace Tamro.Madam.Application.Handlers.Sales.Sabis;

[RequiresPermission(Permissions.CanManageSabisContracts)]
public class DeleteSksContractsCommandHandler : IRequestHandler<DeleteSksContractsCommand, Result<int>>
{
    private readonly IE1DbContext _dbContext;

    public DeleteSksContractsCommandHandler(IE1DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<int>> Handle(DeleteSksContractsCommand command, CancellationToken cancellationToken)
    {
        var mappingsToDelete = await _dbContext.SksContractMappings
            .Where(x => command.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var mapping in mappingsToDelete)
        {
            _dbContext.SksContractMappings.Remove(mapping);
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(result);
    }
}
