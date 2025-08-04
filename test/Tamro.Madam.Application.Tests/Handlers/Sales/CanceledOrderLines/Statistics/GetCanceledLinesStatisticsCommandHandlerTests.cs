using AutoFixture;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Repositories.Sales;

namespace Tamro.Madam.Application.Tests.Handlers.Sales.CanceledOrderLines.Statistics;

[TestFixture]
public class GetCanceledLinesStatisticsCommandHandlerTests
{
    private Fixture _fixture;

    private Mock<ISalesOrderCustomerDecorator> _canceledOrderCustomerDecorator;
    private Mock<ICanceledOrderLineItemDecorator> _canceledOrderLineItemDecorator;
    private Mock<ICanceledLineStatisticRepository> _canceledLineStatisticRepository;

    private GetCanceledLinesStatisticsCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _canceledOrderCustomerDecorator = new Mock<ISalesOrderCustomerDecorator>();
        _canceledOrderLineItemDecorator = new Mock<ICanceledOrderLineItemDecorator>();
        _canceledLineStatisticRepository = new Mock<ICanceledLineStatisticRepository>();

        _handler = new GetCanceledLinesStatisticsCommandHandler(_canceledOrderCustomerDecorator.Object, _canceledOrderLineItemDecorator.Object, _canceledLineStatisticRepository.Object);
    }

    [Test]
    public async Task Handle_CallsGetStatisticsWithCorrectParameters()
    {
        // Arrange
        var request = _fixture.Create<GetCanceledLinesStatisticsCommand>();

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _canceledLineStatisticRepository.Verify(x => x.GetStatistics(request.Country, request.DateFrom, request.DateTill), Times.Once);
    }

    [Test]
    public async Task Handle_StatisticsExist_DecoratesItemAndCustomer()
    {
        // Arrange
        var request = _fixture.Create<GetCanceledLinesStatisticsCommand>();
        var statistics = _fixture.CreateMany<CanceledLineStatisticModel>().ToList();

        _canceledLineStatisticRepository
            .Setup(x => x.GetStatistics(request.Country, request.DateFrom, request.DateTill))
            .ReturnsAsync(statistics);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _canceledOrderLineItemDecorator.Verify(x => x.Decorate(statistics, request.Country), Times.Once);
        _canceledOrderCustomerDecorator.Verify(x => x.Decorate(statistics, request.Country), Times.Once);
    }

    [Test]
    public async Task Handle_NoStatisticsExist_DoesNotDecorate()
    {
        // Arrange
        var request = _fixture.Create<GetCanceledLinesStatisticsCommand>();
        var statistics = new List<CanceledLineStatisticModel>();

        _canceledLineStatisticRepository
            .Setup(x => x.GetStatistics(request.Country, request.DateFrom, request.DateTill))
            .ReturnsAsync(statistics);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _canceledOrderLineItemDecorator.Verify(x => x.Decorate(It.IsAny<IEnumerable<CanceledLineStatisticModel>>(), It.IsAny<BalticCountry>()), Times.Never);
        _canceledOrderCustomerDecorator.Verify(x => x.Decorate(It.IsAny<IEnumerable<CanceledLineStatisticModel>>(), It.IsAny<BalticCountry>()), Times.Never);
    }
}
