using Tamro.Madam.Models.Finance.Peppol;

namespace Tamro.Madam.Repository.Entities.Finance.Peppol;
public class PeppolInvoiceBase
{
    public long Id { get; set; }
    public string CustomerName { get; set; }
    public int InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime InvoiceDueDate { get; set; }
    public decimal TotalInvoiceTaxAmount { get; set; }
    public decimal TotalInvoiceAmountWithoutTax { get; set; }
    public decimal TotalInvoiceAmountWithTax { get; set; }
    public string SellerName { get; set; }
    public string SellerRegNo { get; set; }
    public PeppolInvoiceStatus Status { get; set; }
    public string? ConsolidationNumber { get; set; }
    public PeppolInvoiceType Type { get; set; }
    public DateTime CreatedDate { get; set; }
}
