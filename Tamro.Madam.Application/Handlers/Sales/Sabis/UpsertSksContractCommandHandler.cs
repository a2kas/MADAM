using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.Sabis;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Application.Handlers.Sales.Sabis;

[RequiresPermission(Permissions.CanManageSabisContracts)]
public class UpsertSksContractCommandHandler : IRequestHandler<UpsertSksContractCommand, Result<int>>
{
    private readonly IE1DbContext _context;
    private readonly IMapper _mapper;

    public UpsertSksContractCommandHandler(IE1DbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertSksContractCommand request, CancellationToken cancellationToken)
    {
        if (request.Model.Id == default)
        {
            return await CreateContractAsync(request.Model, cancellationToken);
        }
        else
        {
            return await UpdateContractAsync(request.Model, cancellationToken);
        }
    }

    private async Task<Result<int>> CreateContractAsync(SksContractModel model, CancellationToken cancellationToken)
    {
        var newContract = _mapper.Map<SksContractMapping>(model);
        _context.SksContractMappings.Add(newContract);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(newContract.Id);
    }

    private async Task<Result<int>> UpdateContractAsync(SksContractModel model, CancellationToken cancellationToken)
    {
        var existingSksContract = await _context.SksContractMappings.SingleOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
        if (existingSksContract == null)
        {
            return Result<int>.Failure($"Failed to update sks contract '{model.ContractTamro}', it does not exist");
        }

        _mapper.Map(model, existingSksContract);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(existingSksContract.Id);
    }
}
