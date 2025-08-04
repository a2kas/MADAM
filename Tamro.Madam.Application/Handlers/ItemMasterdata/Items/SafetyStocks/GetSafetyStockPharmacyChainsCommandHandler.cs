using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanViewSafetyStock)]
public class GetSafetyStockPharmacyChainsCommandHandler : IRequestHandler<GetSafetyStockPharmacyChainsCommand, Result<List<PharmacyChainModel>>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetSafetyStockPharmacyChainsCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<List<PharmacyChainModel>>> Handle(GetSafetyStockPharmacyChainsCommand command, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<SafetyStockPharmacyChain>()
            .AsReadOnlyQueryable();
        
        if (command.BalticCountry != default)
        {
            query = query.Where(x => x.Country == command.BalticCountry);
        }
        if (command.IsActive != default)
        {
            query = query.Where(x => x.IsActive == command.IsActive);
        }

        var result = _mapper.Map<List<PharmacyChainModel>>(await query.ToListAsync(cancellationToken));

        return Result<List<PharmacyChainModel>>.Success(result);
    }
}
