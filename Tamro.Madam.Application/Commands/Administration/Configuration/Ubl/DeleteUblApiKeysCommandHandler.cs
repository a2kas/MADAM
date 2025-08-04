using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;

[RequiresPermission(Permissions.CanManageUnifiedPost)]
public class DeleteUblApiKeysCommandHandler : IRequestHandler<DeleteUblApiKeysCommand, Result<int>>
{
    private readonly IUblApiKeyRepository _ublApiKeyRepository;

    public DeleteUblApiKeysCommandHandler(IUblApiKeyRepository ublApiKeyRepository)
    {
        _ublApiKeyRepository = ublApiKeyRepository;
    }

    public async Task<Result<int>> Handle(DeleteUblApiKeysCommand command, CancellationToken cancellationToken)
    {
        var result = await _ublApiKeyRepository.DeleteMany(command.E1SoldTos, cancellationToken);
        return Result<int>.Success(result);
    }
}
