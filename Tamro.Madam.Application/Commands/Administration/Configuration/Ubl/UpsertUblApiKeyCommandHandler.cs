using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;

[RequiresPermission(Permissions.CanManageUnifiedPost)]
public class UpsertUblApiKeyCommandHandler : IRequestHandler<UpsertUblApiKeyCommand, Result<int>>
{
    private readonly IUblApiKeyRepository _ublApiKeyRepository;
    private readonly IMapper _mapper;

    public UpsertUblApiKeyCommandHandler(IUblApiKeyRepository ublApiKeyRepository, IMapper mapper)
    {
        _ublApiKeyRepository = ublApiKeyRepository;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertUblApiKeyCommand command, CancellationToken cancellationToken)
    {
        var result = await _ublApiKeyRepository.Upsert(_mapper.Map<UblApiKeyModel>(command.Model));

        return Result<int>.Success(result.E1SoldTo);
    }
}
