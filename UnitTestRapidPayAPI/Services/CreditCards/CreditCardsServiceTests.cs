using Moq;
using NUnit.Framework;
using AutoMapper;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Services.CreditCards;
using RapidPayAPI.Services.CreditCards.Models;
using System.Threading.Tasks;
using System.Transactions;

namespace RapidPayAPI.Tests.Services.CreditCards
{
    [TestFixture]
    public class CreditCardsServiceTests
    {
        private Mock<ICreditCardsRepository> _mockCreditCardsRepository;
        private Mock<IMapper> _mockMapper;
        private CreditCardsService _creditCardsService;

        [SetUp]
        public void Setup()
        {
            _mockCreditCardsRepository = new Mock<ICreditCardsRepository>();
            _mockMapper = new Mock<IMapper>();
            _creditCardsService = new CreditCardsService(_mockCreditCardsRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task AddCreditCardAsync_ValidRequest_AddsCreditCardAndReturnsResult()
        {
            // Arrange
            var creditCardInput = new CreditCardRequest { Number = "1234567890123456" };
            var creditCard = new CreditCard { Number = creditCardInput.Number, CreditLimit = Constants.LineOfCredit, AvailableCredit = Constants.LineOfCredit };
            var creditCardResult = new CreditCardResult { Number = creditCard.Number };

            _mockMapper.Setup(m => m.Map<CreditCard>(creditCardInput)).Returns(creditCard);
            _mockMapper.Setup(m => m.Map<CreditCardResult>(creditCard)).Returns(creditCardResult);
            _mockCreditCardsRepository.Setup(r => r.AddCreditCardAsync(It.IsAny<CreditCard>())).Returns(Task.CompletedTask);

            // Act
            var result = await _creditCardsService.AddCreditCardAsync(creditCardInput);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(creditCardResult.Number, result.Number);
            _mockCreditCardsRepository.Verify(r => r.AddCreditCardAsync(It.Is<CreditCard>(c => c.Number == creditCard.Number && c.CreditLimit == Constants.LineOfCredit)), Times.Once);
            _mockMapper.Verify(m => m.Map<CreditCardResult>(It.IsAny<CreditCard>()), Times.Once);
        }

        [Test]
        public async Task GetCardBalanceAsync_ValidRequest_ReturnsCardBalance()
        {
            // Arrange
            var cardBalanceRequest = new CardBalanceRequest { Number = "1234567890123456" };
            var creditCard = new CreditCard { Number = cardBalanceRequest.Number, AvailableCredit = 5000 };
            var cardBalanceResult = new CardBalanceResult { Number = creditCard.Number, AvailableCredit = creditCard.AvailableCredit };

            _mockCreditCardsRepository.Setup(r => r.GetCreditCardAsync(cardBalanceRequest.Number)).ReturnsAsync(creditCard);
            _mockMapper.Setup(m => m.Map<CardBalanceResult>(creditCard)).Returns(cardBalanceResult);

            // Act
            var result = await _creditCardsService.GetCardBalanceAsync(cardBalanceRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cardBalanceResult.AvailableCredit, result.AvailableCredit);
            _mockCreditCardsRepository.Verify(r => r.GetCreditCardAsync(cardBalanceRequest.Number), Times.Once);
            _mockMapper.Verify(m => m.Map<CardBalanceResult>(It.IsAny<CreditCard>()), Times.Once);
        }

    }
}
