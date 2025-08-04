namespace Tamro.Madam.Repository.Entities.Finance.Peppol;

public class PeppolInvoice : PeppolInvoiceBase
{
    public List<PeppolInvoiceConsolidated> Invoices { get; set; }
}
