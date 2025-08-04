using FluentValidation;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Application.Validation.Validators.Sales.CanceledOrderLines;
public class CanceledLineStatisticsFilterDatesModelValidator : AbstractValidator<CanceledILinesFilterDatesModel>
{
    public CanceledLineStatisticsFilterDatesModelValidator()
    {
        RuleFor(x => x)
            .Must(y => y.DateFrom < y.DateTill)
            .WithMessage(z => $"{nameof(z.DateFrom)} must be earlier than {nameof(z.DateTill)}");

        RuleFor(x => x)
            .Must(y => (y.DateTill - y.DateFrom).TotalDays <= 30)
            .WithMessage("The date range cannot exceed 30 days.");
    }
}
