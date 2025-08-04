using System.Linq.Expressions;
using System.Reflection;
using AutoFixture;
using AutoMapper;
using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Profiles.ItemMasterdata;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Barcodes;

[TestFixture]
public class GetItemBarcodesCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IBarcodeRepository> _barcodeRepository;
    private IMapper _mapper;

    private GetItemBarcodesCommandHandler _getItemBarcodesCommandHandler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _barcodeRepository = _mockRepository.Create<IBarcodeRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(BarcodeProfile)))));

        _getItemBarcodesCommandHandler = new GetItemBarcodesCommandHandler(_barcodeRepository.Object, _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task Handle_GetsBarcodes_Correctly()
    {
        // Arrange
        const int itemId = 6;
        const int barcodeCount = 2;
        var command = new GetItemBarcodesCommand(itemId);
        var barcodes = _fixture.CreateMany<Barcode>(barcodeCount);
        _barcodeRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Barcode, bool>>>(), It.IsAny<List<IncludeOperation<Barcode>>>(), false, CancellationToken.None)).ReturnsAsync(barcodes);

        // Act
        var result = await _getItemBarcodesCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.Count().ShouldBe(barcodeCount);
    }
}