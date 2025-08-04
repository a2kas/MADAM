using AutoFixture;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Madam;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.Draft.NewProductOffers;

[TestFixture]
public class UpsertNewProductOfferCommandValidatorTests : BaseValidatorTests<UpsertNewProductOfferCommandValidator>
{
    public override void Init()
    {
        base.Init();
        _fixture.Customizations.Add(new MemoryStreamSpecimenBuilder());
    }

    public UpsertNewProductOfferCommand CreateValidCommand()
    {
        var command = _fixture.Create<UpsertNewProductOfferCommand>();
        command.RowVer = DateTime.UtcNow.AddMicroseconds(-1); // ensure the date is in the past
        command.File.Name = "sheet.xlsx";
        return command;
    }

    [Test]
    public void Should_Have_Error_When_SupplierId_Is_Zero()
    {
        var command = CreateValidCommand();
        command.SupplierId = 0;
        _validator.ShouldHaveValidationErrorFor(command, x => x.SupplierId);
    }

    [Test]
    [TestCase("sheet.xls")]
    [TestCase("sheet.xlsx")]
    public void Should_Not_Have_Error_When_Model_Is_Valid(string filename)
    {
        var command = CreateValidCommand();
        command.File.Name = filename;
        _validator.ShouldBeValid(command);
    }

    [Test]
    public void Should_Have_Error_When_ItemCategoryManagerId_Is_Zero()
    {
        var command = CreateValidCommand();
        command.ItemCategoryManagerId = 0;
        _validator.ShouldHaveValidationErrorFor(command, x => x.ItemCategoryManagerId);
    }

    [Test]
    public void Should_Have_Error_When_Country_Is_Unset()
    {
        var command = CreateValidCommand();
        command.Country = (BalticCountry)999;
        _validator.ShouldHaveValidationErrorFor(command, x => x.Country);
    }

    [Test]
    public void Should_Have_Error_When_File_Is_Null()
    {
        var command = CreateValidCommand();
        command.File = null;
        _validator.ShouldHaveValidationErrorFor(command, x => x.File);
    }

    [Test]
    public void Should_Have_Error_When_File_Is_Not_Excel()
    {
        var command = CreateValidCommand();
        command.File = new FileWithName { Name = "test.txt", Stream = new MemoryStream(new byte[] { 0x01, 0x02, 0x03 }) };
        _validator.ShouldHaveValidationErrorFor(command, x => x.File);
    }

    [Test]
    public void Should_Have_Error_When_File_Exceeds_Max_Size()
    {
        var command = CreateValidCommand();
        command.File = new FileWithName { Name = "test.xlsx", Stream = new MemoryStream(new byte[30 * 1024 * 1024 + 1]) };
        _validator.ShouldHaveValidationErrorFor(command, x => x.File);
    }

    [Test]
    public void Should_Have_Error_When_RowVer_Is_Future_Date()
    {
        var command = CreateValidCommand();
        command.RowVer = DateTime.UtcNow.AddDays(1);
        _validator.ShouldHaveValidationErrorFor(command, x => x.RowVer);
    }
}