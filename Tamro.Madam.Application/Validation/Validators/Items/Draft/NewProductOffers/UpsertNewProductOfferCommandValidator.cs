using FluentValidation;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Validation.Validators.Madam;
public class UpsertNewProductOfferCommandValidator : AbstractValidator<UpsertNewProductOfferCommand>
{
    public UpsertNewProductOfferCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .GreaterThan(0);

        RuleFor(x => x.ItemCategoryManagerId)
            .GreaterThan(0);

        RuleFor(x => x.Country)
            .IsInEnum();

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File with the new product offer must be provided.")
            .Must(file => IsValidExcelFile(file))
            .WithMessage("File must be a valid MS Excel file.");

        RuleFor(x => x.File)
            .Must(file => IsBelowMaxSizeFile(file)).When(cmd => cmd.File != null && cmd.File.Stream != null)
            .WithMessage("File with the new product offer must be smaller than 30 MB");

        RuleFor(x => x.RowVer)
            .Must(rowVer => rowVer == null || rowVer <= DateTime.UtcNow)
            .WithMessage("RowVer must be a valid date in the past or null.");
    }

    private static bool IsValidExcelFile(FileWithName file)
    {
        var validExtensions = new[] { ".xls", ".xlsx" };
        var fileExtension = Path.GetExtension(file?.Name ?? string.Empty);
        return validExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsBelowMaxSizeFile(FileWithName file)
    {
        return file.Stream.Length < 30 * 1024 * 1024;
    }
}
