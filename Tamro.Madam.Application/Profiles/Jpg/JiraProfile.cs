using AutoMapper;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;

namespace Tamro.Madam.Application.Profiles.Jpg;
public class JiraProfile : Profile
{
    public JiraProfile()
    {
        CreateMap<JiraAccount, JiraAccountModel>().ReverseMap();
    }
}
