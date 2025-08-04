using System.Collections.Immutable;
using Tamro.Madam.Models.Finance.Peppol;

namespace Tamro.Madam.Models.Data.Finance.Peppol;

public static class PeppolData
{
    public static ImmutableDictionary<string, string> InvoiceStatusDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
       .Add(PeppolInvoiceStatus.Sent.ToString(), nameof(PeppolInvoiceStatus.Sent))
       .Add(PeppolInvoiceStatus.NotSent.ToString(), nameof(PeppolInvoiceStatus.NotSent))
       .Add(PeppolInvoiceStatus.Error.ToString(), nameof(PeppolInvoiceStatus.Error));

    public static ImmutableDictionary<string, string> PeppolInvoiceTypeDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
    .Add(PeppolInvoiceType.Regular.ToString(), PeppolInvoiceType.Regular.ToString())
    .Add(PeppolInvoiceType.Consolidated.ToString(), PeppolInvoiceType.Consolidated.ToString());
}
