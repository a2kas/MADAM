using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Infrastructure.Session;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings.Retail;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class GenerateRetailCodesCommandHandler : IRequestHandler<GenerateRetailCodesCommand, Result<List<GeneratedRetailCodeModel>>>
{
    private readonly IGeneratedRetailCodeRepository _repository;
    private readonly IUserContext _userContext;

    public GenerateRetailCodesCommandHandler(IGeneratedRetailCodeRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<Result<List<GeneratedRetailCodeModel>>> Handle(GenerateRetailCodesCommand request, CancellationToken cancellationToken)
    {
        var latestGeneratedCode = await _repository.GetLatestCode(request.Model.Country.Value, request.Model.CodePrefix);

        var generatedCodes = new List<GeneratedRetailCodeModel>();
        for (int i = 1; i <= request.Model.AmountToGenerate; i++)
        {
            var generatedCode = new GeneratedRetailCodeModel()
            {
                Code = latestGeneratedCode + i,
                CodePrefix = request.Model.CodePrefix,
                Country = request.Model.Country.Value,
                GeneratedBy = _userContext.DisplayName,
            };
            generatedCodes.Add(generatedCode);
        }

        await _repository.InsertMany(generatedCodes, cancellationToken);
       
        return Result<List<GeneratedRetailCodeModel>>.Success(generatedCodes);
    }
}