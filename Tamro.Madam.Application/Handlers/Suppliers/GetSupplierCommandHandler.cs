using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Suppliers;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Repositories.Suppliers;

namespace Tamro.Madam.Application.Handlers.Suppliers;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetSupplierCommandHandler : IRequestHandler<GetSupplierCommand, Result<SupplierDetailsModel>>
{
    private readonly ISupplierRepository _repository;

    public GetSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SupplierDetailsModel>> Handle(GetSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.Get(request.Id);

        return Result<SupplierDetailsModel>.Success(supplier);
    }
}
