using AutoFixture;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Items.QualityCheck;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Tests.Services.Items.QualityCheck;

[TestFixture]
public class ActualValueResolverTests
{
    private Fixture _fixture;
    private ActualValueResolver _resolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _resolver = new ActualValueResolver();
    }

    [Test]
    [TestCase("strength", "StrengthValue")]
    [TestCase("activeIngredient", "AtcValue")]
    [TestCase("name", "ItemNameValue")]
    [TestCase("shortDescription", "ShortDescriptionValue")]
    [TestCase("fullDescription", "DescriptionValue")]
    [TestCase("usage", "UsageValue")]
    [TestCase("unknownField", "")]
    public void ResolveActualValue_ShouldReturnExpectedValue(string field, string expectedValue)
    {
        // Arrange
        var binding = _fixture.Build<ItemBindingQualityCheckReferenceModel>()
            .With(b => b.ShortDescription, "ShortDescriptionValue")
            .With(b => b.Description, "DescriptionValue")
            .With(b => b.Usage, "UsageValue")
            .Create();

        var item = _fixture.Build<ItemQualityCheckReferenceModel>()
            .With(i => i.Strength, "StrengthValue")
            .With(i => i.Atc, "AtcValue")
            .With(i => i.ItemName, "ItemNameValue")
            .Create();

        // Act
        var actualValue = _resolver.ResolveActualValue(field, binding, item);

        // Assert
        actualValue.ShouldBe(expectedValue);
    }
}