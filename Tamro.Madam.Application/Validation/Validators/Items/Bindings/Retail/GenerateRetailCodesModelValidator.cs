using FluentValidation;
using Tamro.Madam.Common.Constants;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Validation.Validators.Items.Bindings.Retail;

public class GenerateRetailCodesModelValidator : AbstractValidator<GenerateRetailCodesModel>
{
    public GenerateRetailCodesModelValidator()
    {
        RuleFor(x => x.Country)
            .NotNull()
            .Must(x => x.Value == BalticCountry.LV);

        RuleFor(x => x.CodePrefix)
            .Must(x => x == CommonConstants.LvRetailPrefix);

        RuleFor(x => x.AmountToGenerate)
            .NotEmpty()
            .InclusiveBetween(1, 50);
    }
}
