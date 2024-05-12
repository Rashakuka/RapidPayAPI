using Moq;
using NUnit.Framework;
using AutoMapper;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Repositories.Payments;
using RapidPayAPI.Services.Payments;
using RapidPayAPI.Services.Payments.Models;
using RapidPayAPI.Services.UFEFee;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace RapidPayAPI.Tests.Services.Payments
{
    [TestFixture]
    public class PaymentsServiceTests
    {
        private Mock<IPaymentsRepository> _mockPaymentsRepository;
        private Mock<ICreditCardsRepository> _mockCreditCardsRepository;
        private Mock<UFEFeeService> _mockUFEService;
        private Mock<IMapper> _mockMapper;
        private PaymentsService _paymentsService;

        [SetUp]
        public void Setup()
        {
            _mockPaymentsRepository = new Mock<IPaymentsRepository>();
            _mockCreditCardsRepository = new Mock<ICreditCardsRepository>();
            _mockUFEService = new Mock<UFEFeeService>();
            _mockMapper = new Mock<IMapper>();
            _paymentsService = new PaymentsService(
                _mockPaymentsRepository.Object,
                _mockCreditCardsRepository.Object,
                _mockUFEService.Object,
                _mockMapper.Object);
        }

        [Test]
        public async Task AddPaymentAsync_WhenCreditIsSufficient_ShouldProcessPayment()
        {
            // Arrange
            var paymentRequest = new PaymentRequest { Amount = 100, CreditCardNumber = "1234567890123456" };
            var payment = new Payment { Amount = 100 };
            var creditCard = new CreditCard { AvailableCredit = 200, Id = 1 };

            _mockMapper.Setup(m => m.Map<Payment>(It.IsAny<PaymentRequest>())).Returns(payment);
            _mockUFEService.Setup(s => s.Fee).Returns(0.05m); // Simulate a 5% transaction fee
            _mockCreditCardsRepository.Setup(r => r.GetCreditCardAsync(paymentRequest.CreditCardNumber))
                                      .ReturnsAsync(creditCard);
            _mockPaymentsRepository.Setup(r => r.AddPaymentAsync(It.IsAny<Payment>()))
                                   .Returns(Task.CompletedTask);
            _mockCreditCardsRepository.Setup(r => r.UpdateCreditCardAsync(It.IsAny<CreditCard>()))
                                      .Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PaymentResult>(It.IsAny<Payment>()))
                       .Returns((Payment p) => new PaymentResult { TotalAmount = p.TotalAmount });

            // Act
            var result = await _paymentsService.AddPaymentAsync(paymentRequest);

            // Assert
            Assert.AreEqual(105, result.TotalAmount); // Assert the total amount includes the fee
            _mockCreditCardsRepository.Verify(r => r.UpdateCreditCardAsync(It.Is<CreditCard>(c => c.AvailableCredit == 95)));
            _mockMapper.Verify(m => m.Map<PaymentResult>(It.Is<Payment>(p => p.TotalAmount == 105)));
        }

    }
}
