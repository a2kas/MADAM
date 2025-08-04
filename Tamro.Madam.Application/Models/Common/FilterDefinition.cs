namespace Tamro.Madam.Application.Models.Common;

public class FilterDefinition
{
    public required string Column { get; set; }

    public required string Operator { get; set; }

    public required object Value { get; set; }
}
