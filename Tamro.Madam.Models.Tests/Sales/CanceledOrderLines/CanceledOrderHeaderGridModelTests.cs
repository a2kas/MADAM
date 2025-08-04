using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Models.Tests.Sales.CanceledOrderLines;

[TestFixture]
public class CanceledOrderHeaderGridModelTests
{
    [Test]
    public void CanceledOrderHeaderGridModel_HasNoLines_EmailStatusIsSent()
    {
        // Arrange + act
        var model = new CanceledOrderHeaderGridModel();

        // Assert
        model.EmailStatus.ShouldBe(CanceledOrderHeaderEmailStatus.Sent);
    }

    [Test]
    public void CanceledOrderHeaderGridModel_AllLinesHasTheSameStatus_ReturnsTheSameStatus()
    {
        // Arrange + act
        var model = new CanceledOrderHeaderGridModel()
        {
            Lines =
            [
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.WillNotBeSent,
                },
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.WillNotBeSent,
                }
            ]
        };

        // Assert
        model.EmailStatus.ShouldBe(CanceledOrderHeaderEmailStatus.WillNotBeSent);
    }

    [Test]
    public void CanceledOrderHeaderGridModel_OnlyOneLineIsSent_ReturnsPartiallySent()
    {
        // Arrange + act
        var model = new CanceledOrderHeaderGridModel()
        {
            Lines =
            [
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.Sent,
                },
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.FailureSending,
                }
            ]
        };

        // Assert
        model.EmailStatus.ShouldBe(CanceledOrderHeaderEmailStatus.PartiallySent);
    }

    [Test]
    public void CanceledOrderHeaderGridModel_AtLeastOneLineIsSent_ReturnsPartiallySent()
    {
        // Arrange + act
        var model = new CanceledOrderHeaderGridModel()
        {
            Lines =
            [
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.WillNotBeSent,
                },
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.Sent,
                },
                new CanceledOrderLineGridModel() 
                {
                    EmailStatus = CanceledOrderLineEmailStatus.NotSent,
                }
            ]
        };

        // Assert
        model.EmailStatus.ShouldBe(CanceledOrderHeaderEmailStatus.PartiallySent);
    }

    [Test]
    public void CanceledOrderHeaderGridModel_OnlyNotSentAndFailureSendingLines_ReturnsNotSent()
    {
        // Arrange + act
        var model = new CanceledOrderHeaderGridModel()
        {
            Lines =
            [
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.NotSent,
                },
                new CanceledOrderLineGridModel()
                {
                    EmailStatus = CanceledOrderLineEmailStatus.FailureSending,
                }
            ]
        };

        // Assert
        model.EmailStatus.ShouldBe(CanceledOrderHeaderEmailStatus.NotSent);
    }
}