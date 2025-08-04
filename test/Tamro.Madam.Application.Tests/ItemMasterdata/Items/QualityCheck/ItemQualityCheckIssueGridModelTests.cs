using AutoFixture;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Tests.ItemMasterdata.Items.QualityCheck;

[TestFixture]
public class ItemQualityCheckIssueGridModelTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void IssueField_ShouldCapitalizeAndFormatCorrectly()
    {
        // Arrange
        var model = _fixture.Create<ItemQualityCheckIssueGridModel>();
        model.IssueField = "someFieldName";

        // Act
        var formattedField = model.IssueField;

        // Assert
        formattedField.ShouldBe("Some Field Name");
    }

    [Test]
    public void IssueField_ShouldReturnEmptyString_WhenInputIsEmpty()
    {
        // Arrange
        var model = _fixture.Create<ItemQualityCheckIssueGridModel>();
        model.IssueField = string.Empty;

        // Act
        var formattedField = model.IssueField;

        // Assert
        formattedField.ShouldBe(string.Empty);
    }

    [Test]
    public void Identifier_ShouldReturnItemBindingId_WhenItemBindingIdIsNotNullOrEmpty()
    {
        // Arrange
        var model = _fixture.Create<ItemQualityCheckIssueGridModel>();
        model.ItemBindingId = "Binding123";
        model.ItemId = 456;

        // Act
        var identifier = model.Identifier;

        // Assert
        identifier.ShouldBe("Binding123");
    }

    [Test]
    public void Identifier_ShouldReturnItemId_WhenItemBindingIdIsNullOrEmpty()
    {
        // Arrange
        var model = _fixture.Create<ItemQualityCheckIssueGridModel>();
        model.ItemBindingId = null;
        model.ItemId = 456;

        // Act
        var identifier = model.Identifier;

        // Assert
        identifier.ShouldBe("456");
    }
}