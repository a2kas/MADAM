using AutoMapper;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Repository.Entities.Finance.Peppol;

namespace Tamro.Madam.Application.Profiles.Finance.Peppol;

public class PeppolInvoiceProfile : Profile
{
    public PeppolInvoiceProfile()
    {
        CreateMap<PeppolInvoice, PeppolInvoiceGridModel>()
            .ForMember(d => d.FileReference, o => o.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber == 0 ? string.Empty : src.InvoiceNumber.ToString()))
            .ForMember(dest => dest.ConsolidationNumber, opt => opt.MapFrom(src => src.ConsolidationNumber ?? string.Empty));
        CreateMap<PeppolInvoiceConsolidated, PeppolInvoiceConsolidatedGridModel>();
    }
}
