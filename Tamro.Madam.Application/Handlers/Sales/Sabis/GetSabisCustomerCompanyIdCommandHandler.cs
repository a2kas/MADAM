using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.Sabis;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Context.E1Gateway;

namespace Tamro.Madam.Application.Handlers.Sales.Sabis;

[RequiresPermission(Permissions.CanViewSabisContracts)]
public class GetSabisCustomerCompanyIdCommandHandler : IRequestHandler<GetSabisCustomerCompanyIdCommand, Result<string>>
{
    private readonly IE1DbContext _dbContext;

    public GetSabisCustomerCompanyIdCommandHandler(IE1DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<string>> Handle(GetSabisCustomerCompanyIdCommand command, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(x => x.AddressNumber == command.AddressNumber);

        return Result<string>.Success(customer?.AdditionalTaxId);
    }
}
