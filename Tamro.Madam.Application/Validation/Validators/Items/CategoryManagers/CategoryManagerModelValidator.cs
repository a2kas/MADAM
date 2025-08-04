using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;

namespace Tamro.Madam.Application.Validation.Validators.Madam;
public class CategoryManagerModelValidator : AbstractValidator<CategoryManagerModel>
{
    public CategoryManagerModelValidator()
    {
        RuleFor(x => x.EmailAddress)
            .EmailAddress()
            .MaximumLength(50)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .MaximumLength(30)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .MaximumLength(30)
            .NotEmpty();

        RuleFor(x => x.Country)
            .IsInEnum()
            .NotNull();
    }
}
