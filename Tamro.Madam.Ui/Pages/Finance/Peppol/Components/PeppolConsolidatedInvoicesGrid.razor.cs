using Microsoft.AspNetCore.Components;
using Tamro.Madam.Models.Finance.Peppol;

namespace Tamro.Madam.Ui.Pages.Finance.Peppol.Components;

public partial class PeppolConsolidatedInvoicesGrid
{
    [EditorRequired]
    [Parameter]
    public List<PeppolInvoiceConsolidatedGridModel> Invoices { get; set; }
}
