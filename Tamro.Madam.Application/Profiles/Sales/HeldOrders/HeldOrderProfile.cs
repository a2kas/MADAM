using AutoMapper;
using Tamro.Madam.Models.Employees.Wholesale;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Profiles.Sales.HeldOrders;
public class HeldOrderProfile : Profile
{
    public HeldOrderProfile()
    {
        CreateMap<E1HeldOrder, E1HeldOrderModel>()
            .ForMember(d => d.OldNotificationStatus, o => o.MapFrom(s => s.NotificationStatus))
            .ForMember(d => d.OldNotificationSendDate, o => o.MapFrom(s => s.NotificationSendDate))
            .ForMember(d => d.MailingName, o => o.Ignore())
            .ForMember(d => d.ResponsibleEmployeeNumber, o => o.Ignore())
            .ForMember(d => d.EmployeesEmail, o => o.Ignore())
            .ForMember(d => d.HasValidCustomerEmails, o => o.Ignore())
            .ForMember(d => d.HasValidEmployeeEmails, o => o.Ignore());
        CreateMap<E1HeldOrderModel, E1HeldOrder>();
        CreateMap<LvWholesaleEmployee, WholesaleEmployeeModel>();
        CreateMap<E1HeldOrder, HeldOrderGridModel>()
            .ForMember(d => d.CustomerName, o => o.Ignore());
    }
}
