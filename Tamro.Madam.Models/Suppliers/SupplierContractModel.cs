using System.ComponentModel;

namespace Tamro.Madam.Models.Suppliers;

public class SupplierContractModel
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public long Id { get; set; }

    [DisplayName("Contract date")]
    public DateTime? AgreementDate { get; set; }

    [DisplayName("Valid from")]
    public DateTime? AgreementValidFrom { get; set; }

    [DisplayName("Valid to")]
    public DateTime? AgreementValidTo { get; set; }

    [DisplayName("Payment term (days)")]
    public int? PaymentTermInDays { get; set; }

    [DisplayName("File")]
    public string? DocumentReference { get; set; }

    [DisplayName("Status")]
    public SupplierContractStatus Status
    {
        get
        {
            DateTime currentDate = DateTime.Now;

            if (AgreementValidFrom == null)
            {
                if (AgreementValidTo == null)
                {
                    return SupplierContractStatus.Unknown;
                }

                return currentDate > AgreementValidTo ? SupplierContractStatus.Expired : SupplierContractStatus.Unknown;
            }

            if (AgreementValidTo == null)
            {
                return currentDate >= AgreementValidFrom ? SupplierContractStatus.Active : SupplierContractStatus.Upcoming;
            }

            if (AgreementValidFrom > AgreementValidTo)
            {
                return SupplierContractStatus.Invalid;
            }

            if (currentDate < AgreementValidFrom)
            {
                return SupplierContractStatus.Upcoming;
            }

            if (currentDate > AgreementValidTo)
            {
                return SupplierContractStatus.Expired;
            }

            if ((currentDate >= AgreementValidFrom) && (currentDate <= AgreementValidTo))
            {
                return SupplierContractStatus.Active;
            }

            // This code should be impossible to reach.
            return SupplierContractStatus.Unknown;
        }
    }
}
