using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Models.Tests.Suppliers;

[TestFixture]
public class SupplierContractModelTests
{
    [Test]
    public void Status_CurrentDateIsBetweenAgreementValidFromAndAgreementValidTo_StatusShouldBeActive()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = DateTime.Now.AddYears(-1),
            AgreementValidTo = DateTime.Now.AddYears(1)
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Active);
    }

    [Test]
    public void Status_AgreementValidFromIsNullAndCurrentDateIsAfterAgreementValidTo_StatusShouldBeExpired()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = null,
            AgreementValidTo = DateTime.Now.AddYears(-1)
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Expired);
    }

    [Test]
    public void Status_AgreementValidToIsNullAndCurrentDateIsBeforeAgreementValidFrom_StatusShouldBeUpcoming()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = DateTime.Now.AddYears(1),
            AgreementValidTo = null
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Upcoming);
    }

    [Test]
    public void Status_AgreementValidFromIsNullAndAgreementValidToIsNull_StatusShouldBeUnknown()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = null,
            AgreementValidTo = null
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Unknown);
    }

    [Test]
    public void Status_AgreementValidToIsNullAndCurrentDateIsAfterAgreementValidFrom_StatusShouldBeActive()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = DateTime.Now.AddYears(-1),
            AgreementValidTo = null
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Active);
    }

    [Test]
    public void Status_AgreementValidFromIsAfterAgreementValidTo_StatusShouldBeInvalid()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = DateTime.Now.AddYears(1),
            AgreementValidTo = DateTime.Now.AddYears(-1)
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Invalid);
    }

    [Test]
    public void Status_CurrentDateIsBeforeAgreementValidFrom_StatusShouldBeUpcoming()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = DateTime.Now.AddYears(1),
            AgreementValidTo = DateTime.Now.AddYears(2)
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Upcoming);
    }

    [Test]
    public void Status_CurrentDateIsAfterAgreementValidTo_StatusShouldBeExpired()
    {
        // Arrange + Act
        var model = new SupplierContractModel()
        {
            AgreementValidFrom = DateTime.Now.AddYears(-2),
            AgreementValidTo = DateTime.Now.AddYears(-1)
        };
        // Assert
        model.Status.ShouldBe(SupplierContractStatus.Expired);
    }
}