using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Suppliers;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Repositories.Suppliers;

namespace Tamro.Madam.Application.Handlers.Suppliers;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertSupplierCommandHandler : IRequestHandler<UpsertSupplierCommand, Result<SupplierDetailsModel>>
{
    private readonly ISupplierRepository _repository;

    public UpsertSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SupplierDetailsModel>> Handle(UpsertSupplierCommand request, CancellationToken cancellationToken)
    {
        var upsertedSupplier = await _repository.UpsertGraph(request.Model);

        return Result<SupplierDetailsModel>.Success(upsertedSupplier);
    }
}
