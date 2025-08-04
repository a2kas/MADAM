using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;

namespace Tamro.Madam.Application.Validation.Validators.MeasurementUnits;

public class MeasurementUnitModelValidator : AbstractValidator<MeasurementUnitModel>
{
    public MeasurementUnitModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}


