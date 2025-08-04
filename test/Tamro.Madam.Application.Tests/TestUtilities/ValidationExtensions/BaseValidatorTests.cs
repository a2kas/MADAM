using AutoFixture;
using NUnit.Framework;

namespace Tamro.Madam.Application.Tests.TestExtensions.Validation;

public class BaseValidatorTests<TValidator> where TValidator : class, new()
{
    protected Fixture _fixture;
    protected TValidator _validator;

    [SetUp]
    public virtual void Init()
    {
        _fixture = new Fixture();
        _validator = new TValidator();
    }
}
