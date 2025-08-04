using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Suppliers;

public class Supplier : IMadamEntity<int>, IBaseEntity, IAuditable
{
    [Key]
    public int Id { get; set; }
    public string RegistrationNumber { get; set; }
    public string Name { get; set; }
    public BalticCountry Country { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public ICollection<SupplierContract> Contracts { get; set; }
}
