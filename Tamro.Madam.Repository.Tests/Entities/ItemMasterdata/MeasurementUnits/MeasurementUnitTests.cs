using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.MeasurementUnits;

[TestFixture]
public class MeasurementUnitTests
{
    [Test]
    public void MeasurementUnit_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(MeasurementUnit)).ShouldBeTrue();
    }
}