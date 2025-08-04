using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Suppliers;

public class SupplierContract : IMadamEntity<long>, IBaseEntity, IAuditable
{
    [Key]
    public long Id { get; set; }
    public DateTime? AgreementDate { get; set; }
    public DateTime? AgreementValidFrom { get; set; }
    public DateTime? AgreementValidTo { get; set; }
    public int? PaymentTermInDays { get; set; }
    public string? DocumentReference { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public int SupplierId { get; set; }
}
