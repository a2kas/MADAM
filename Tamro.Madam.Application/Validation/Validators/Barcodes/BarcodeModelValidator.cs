using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Validation.Validators.Barcodes;

public class BarcodeModelValidator : AbstractValidator<BarcodeModel>
{
    public BarcodeModelValidator()
    {
        RuleFor(x => x.Ean)
            .MaximumLength(50)
            .NotEmpty();

        RuleFor(x => x.Item)
            .NotEmpty();
    }
}
