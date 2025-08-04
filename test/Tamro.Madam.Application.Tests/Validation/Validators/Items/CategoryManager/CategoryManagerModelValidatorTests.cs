using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Madam;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.CategoryManager;

[TestFixture]
public class CategoryManagerModelValidatorTests : BaseValidatorTests<CategoryManagerModelValidator>
{

    [Test]
    public void Should_Have_Error_When_EmailAddress_Is_Empty()
    {
        var model = new CategoryManagerModel { EmailAddress = string.Empty };
        _validator.ShouldHaveValidationErrorFor(model, m => m.EmailAddress);
    }

    [Test]
    public void Should_Have_Error_When_EmailAddress_Exceeds_MaximumLength()
    {
        var model = new CategoryManagerModel { EmailAddress = new string('a', 51) };
        _validator.ShouldHaveValidationErrorFor(model, x => x.EmailAddress);
    }

    [Test]
    public void Should_Not_Have_Error_When_EmailAddress_Is_Valid()
    {
        var model = new CategoryManagerModel { EmailAddress = "test@example.com" };
        _validator.ShouldNotHaveValidationErrorFor(model, x => x.EmailAddress);
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var model = new CategoryManagerModel { FirstName = string.Empty };
        _validator.ShouldHaveValidationErrorFor(model, x => x.FirstName);
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Exceeds_MaximumLength()
    {
        var model = new CategoryManagerModel { FirstName = new string('a', 31) };
        _validator.ShouldHaveValidationErrorFor(model, x => x.FirstName);
    }

    [Test]
    public void Should_Not_Have_Error_When_FirstName_Is_Valid()
    {
        var model = new CategoryManagerModel { FirstName = "John" };
        _validator.ShouldNotHaveValidationErrorFor(model, x => x.FirstName);
    }

    [Test]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var model = new CategoryManagerModel { LastName = string.Empty };
        _validator.ShouldHaveValidationErrorFor(model, x => x.LastName);
    }

    [Test]
    public void Should_Have_Error_When_LastName_Exceeds_MaximumLength()
    {
        var model = new CategoryManagerModel { LastName = new string('a', 31) };
        _validator.ShouldHaveValidationErrorFor(model, x => x.LastName);
    }

    [Test]
    public void Should_Not_Have_Error_When_LastName_Is_Valid()
    {
        var model = new CategoryManagerModel { LastName = "Doe" };
        _validator.ShouldNotHaveValidationErrorFor(model, x => x.LastName);
    }

    [Test]
    public void Should_Have_Error_When_Country_Is_Unset()
    {
        var model = new CategoryManagerModel { Country = default };
        _validator.ShouldHaveValidationErrorFor(model, x => x.Country);
    }

    [Test]
    public void Should_Not_Have_Error_When_Country_Is_Valid()
    {
        var model = new CategoryManagerModel { Country = BalticCountry.LT };
        _validator.ShouldNotHaveValidationErrorFor(model, x => x.Country);
    }
}
