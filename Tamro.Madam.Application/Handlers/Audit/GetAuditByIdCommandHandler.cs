using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Commands.Audit;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Audit;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Repositories.Audit;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Application.Handlers.Audit;

[NoPermissionNeeded]
public class GetAuditByIdCommandHandler : IRequestHandler<GetAuditByIdCommand, Result<AuditDetailsModel>>
{
    private readonly IAuditRepository _repository;
    private readonly IMapper _mapper;

    public GetAuditByIdCommandHandler(IAuditRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AuditDetailsModel>> Handle(GetAuditByIdCommand request, CancellationToken cancellationToken)
    {
        var includes = new List<IncludeOperation<DbAuditEntry>>
        {
            new(q => q.Include(a => a.Properties)),
        };

        var audit = _repository.Get(request.Id, includes);

        return Result<AuditDetailsModel>.Success(_mapper.Map<AuditDetailsModel>(audit));
    }
}
