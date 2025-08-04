using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Retail;

public class GenerateRetailCodesCommand : IRequest<Result<List<GeneratedRetailCodeModel>>>, IDefaultErrorMessage
{
    public GenerateRetailCodesCommand(GenerateRetailCodesModel model)
    {
        Model = model;
    }

    public GenerateRetailCodesModel Model { get; }
    public string ErrorMessage { get; set; } = "Failed to generate retail codes";
}
