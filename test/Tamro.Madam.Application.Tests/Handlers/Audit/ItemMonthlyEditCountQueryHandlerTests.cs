using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Handlers.Audit;
using Tamro.Madam.Repository.Repositories.Audit;

namespace Tamro.Madam.Application.Tests.Handlers.Audit;

[TestFixture]
public class ItemMonthlyEditCountQueryHandlerTests
{
    private MockRepository _mockRepository;
    private Mock<IAuditRepository> _auditRepository;
    private ItemMonthlyEditCountQueryHandler _itemMonthlyEditCountQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _auditRepository = _mockRepository.Create<IAuditRepository>();

        _itemMonthlyEditCountQueryHandler = new ItemMonthlyEditCountQueryHandler(_auditRepository.Object);
    }

    [Test]
    public async Task Handle_Gets_ItemAuditEntriesCountByMonthForLastYear()
    {
        // Act
        await _itemMonthlyEditCountQueryHandler.Handle(null, CancellationToken.None);

        // Assert
        _auditRepository.Verify(x => x.GetItemAuditEntriesCountByMonthForLastYear(CancellationToken.None), Times.Once);
    }
}