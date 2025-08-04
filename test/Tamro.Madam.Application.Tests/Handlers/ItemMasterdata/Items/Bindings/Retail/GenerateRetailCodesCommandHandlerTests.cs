//using Shouldly;
//using Moq;
//using NUnit.Framework;
//using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Retail;
//using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings.Retail;
//using Tamro.Madam.Application.Infrastructure.Session;
//using Tamro.Madam.Models.General;
//using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
//using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
//using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Retail;

//namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.Bindings.Retail
//{
//    [TestFixture]
//    public class GenerateRetailCodesCommandHandlerTests
//    {
//        private MockRepository _mockRepository;

//        private Mock<IGeneratedRetailCodeRepository> _generatedRetailCodeRepository;
//        private Mock<IUserContext> _userContext;

//        private GenerateRetailCodesCommandHandler _generateRetailCodesCommandHandler;

//        [SetUp]
//        public void SetUp()
//        {
//            _mockRepository = new MockRepository(MockBehavior.Loose);

//            _generatedRetailCodeRepository = _mockRepository.Create<IGeneratedRetailCodeRepository>();
//            _userContext = _mockRepository.Create<IUserContext>();

//            _generateRetailCodesCommandHandler = new GenerateRetailCodesCommandHandler(_generatedRetailCodeRepository.Object, _userContext.Object);
//        }

//        [Test]
//        public async Task Handle_ShouldGenerateRetailCodes_Correctly()
//        {
//            // Arrange
//            var model = new GenerateRetailCodesModel()
//            {
//                CodePrefix = "HS",
//                Country = new CountryModel()
//                {
//                    Name = "LV",
//                    Value = BalticCountry.LV,
//                },
//                AmountToGenerate = 2,
//            };
//            var cmd = new GenerateRetailCodesCommand(model);
//            _generatedRetailCodeRepository.Setup(x => x.GetLatestCode(BalticCountry.LV, "HS")).Returns(5);
//            _userContext.Setup(x => x.DisplayName).Returns("Test User");

//            // Act
//            var result = await _generateRetailCodesCommandHandler.Handle(cmd, CancellationToken.None);

//            // Assert
//            _generatedRetailCodeRepository.Verify(x => x.InsertMany(It.Is<List<GeneratedRetailCodeModel>>(y => y.Count == 2 &&
//                y.Any(t => t.Code == 6 && t.CodePrefix == "HS" && t.Country == BalticCountry.LV && t.GeneratedBy == "Test User") &&
//                y.Any(t => t.Code == 7 && t.CodePrefix == "HS" && t.Country == BalticCountry.LV && t.GeneratedBy == "Test User")), It.IsAny<CancellationToken>()), Times.Once);
//            result.Succeeded.ShouldBeTrue();
//            result.Data.Count.ShouldBe(2);

//        }
//    }
//}