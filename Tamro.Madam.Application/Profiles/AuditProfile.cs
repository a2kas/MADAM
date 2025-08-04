using AutoMapper;
using Tamro.Madam.Models.Audit;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Application.Profiles;

public class AuditProfile : Profile
{
    public AuditProfile()
    {
        CreateMap<DbAuditEntry, AuditGridModel>()
            .ForMember(d => d.Id, o => o.MapFrom(src => src.AuditEntryID))
            .ForMember(d => d.EntityId, o => o.MapFrom(src => src.EntityID))
            .ForMember(d => d.CreatedDate, o => o.MapFrom(src => src.CreatedDate.ToLocalTime()));

        CreateMap<DbAuditEntry, AuditDetailsModel>()
            .ForMember(d => d.Id, o => o.MapFrom(src => src.AuditEntryID))
            .ForMember(d => d.EntityId, o => o.MapFrom(src => src.EntityID));

        CreateMap<DbAuditEntryProperty, AuditPropertyModel>();
    }
}
