namespace Tamro.Madam.Models.Gateways.GptService;

//TODO: To be auto-generated. @MH is checking currently

public class AiExtractedSupplierContract
{
    public ExtractedInformation ExtractedInformation { get; set; }
    public string Model { get; set; }
}
public class ExtractedInformation
{
    public DocumentMetadata DocumentMetadata { get; set; }
    public AgreementParties AgreementParties { get; set; }
    public AgreementTerm AgreementTerm { get; set; }
    public FinancialProvisions FinancialProvisions { get; set; }
}

public class DocumentMetadata
{
    public DateReference DateOfAgreementDocument { get; set; }
}

public class AgreementParties
{
    public ValueReference SupplierName { get; set; }
    public ValueReference SupplierRegistrationNumber { get; set; }
}

public class AgreementTerm
{
    public DateReference AgreementDateValidFrom { get; set; }
    public DateReference AgreementDateValidTo { get; set; }
}

public class FinancialProvisions
{
    public NumberReference PaymentTermInDays { get; set; }
}

public class ValueReference
{
    public string Value { get; set; }
    public string Reference { get; set; }
}

public class DateReference
{
    public DateTime? Value { get; set; }
    public string Reference { get; set; }
}

public class NumberReference
{
    public int Value { get; set; }
    public string Reference { get; set; }
}
