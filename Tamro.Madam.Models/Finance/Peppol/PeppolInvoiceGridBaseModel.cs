using System.ComponentModel;

namespace Tamro.Madam.Models.Finance.Peppol;
public class PeppolInvoiceGridBaseModel
{
    public long Id { get; set; }
    [DisplayName("Customer name")]
    public string CustomerName { get; set; }
    [DisplayName("Invoice number")]
    public string InvoiceNumber { get; set; }
    [DisplayName("Invoice date")]
    public DateTime InvoiceDate { get; set; }
    [DisplayName("Invoice due date")]
    public DateTime InvoiceDueDate { get; set; }
    [DisplayName("Tax amount")]
    public decimal TotalInvoiceTaxAmount { get; set; }
    [DisplayName("Amount without tax")]
    public decimal TotalInvoiceAmountWithoutTax { get; set; }
    [DisplayName("Total amount")]
    public decimal TotalInvoiceAmountWithTax { get; set; }
    [DisplayName("Status")]
    public PeppolInvoiceStatus Status { get; set; }
    [DisplayName("Type")]
    public PeppolInvoiceType Type { get; set; }
    [DisplayName("Seller name")]
    public string SellerName { get; set; }
    [DisplayName("Seller registration number")]
    public string SellerRegNo { get; set; }
    [DisplayName("Created date")]
    public DateTime CreatedDate { get; set; }
}
