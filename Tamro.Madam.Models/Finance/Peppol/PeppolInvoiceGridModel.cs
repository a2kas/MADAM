using System.ComponentModel;

namespace Tamro.Madam.Models.Finance.Peppol;

public class PeppolInvoiceGridModel : PeppolInvoiceGridBaseModel
{
    [DisplayName("Consolidation number")]
    public string ConsolidationNumber { get; set; }
    [DisplayName("File")]
    public string FileReference { get; set; }
    public List<PeppolInvoiceConsolidatedGridModel> Invoices { get; set; }
}
