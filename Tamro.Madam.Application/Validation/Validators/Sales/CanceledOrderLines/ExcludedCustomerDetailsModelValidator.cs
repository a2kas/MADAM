using FluentValidation;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

namespace Tamro.Madam.Application.Validation.Validators.Sales.CanceledOrderLines;

public class ExcludedCustomerDetailsModelValidator : AbstractValidator<ExcludedCustomerDetailsModel>
{
    public ExcludedCustomerDetailsModelValidator()
    {
        RuleFor(x => x.Customer)
            .NotEmpty();
    }
}
