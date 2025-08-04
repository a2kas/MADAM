using System.ComponentModel.DataAnnotations.Schema;

namespace Tamro.Madam.Repository.Entities.Customers.Sks;

[Table("orders_reply_emails")]
public class OrderNotificationEmail
{
    [Column("prodis_cust")]
    public string AddressNumber { get; set; }
    public string Email { get; set; }  
}
