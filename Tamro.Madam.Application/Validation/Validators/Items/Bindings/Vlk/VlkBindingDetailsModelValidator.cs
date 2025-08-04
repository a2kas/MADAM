using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Application.Validation.Validators.Items.Bindings.Vlk;

public class VlkBindingDetailsModelValidator : AbstractValidator<VlkBindingDetailsModel>
{
    public VlkBindingDetailsModelValidator()
    {
        RuleFor(x => x.NpakId7)
            .GreaterThan(0)
            .NotEmpty()
            .WithMessage("'Npak Id7' is required");

        RuleFor(x => x.ItemBinding)
            .Must(x => x != null && x.Id != default)
            .WithMessage("'Item' is required");
    }
}
