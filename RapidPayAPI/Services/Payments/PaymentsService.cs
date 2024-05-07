using AutoMapper;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Repositories.Payments;
using RapidPayAPI.Services.Payments.Models;
using RapidPayAPI.Services.UFEFee;
using System.Transactions;

namespace RapidPayAPI.Services.Payments
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentsRepository;

        private readonly ICreditCardsRepository _creditCardsRepository;

        private readonly UFEFeeService _UFEService;

        private readonly IMapper _mapper;

        public PaymentsService(IPaymentsRepository paymentsRepository,
            ICreditCardsRepository creditCardsRepository,
            UFEFeeService UFEService,
            IMapper mapper)
        {
            _paymentsRepository = paymentsRepository;
            _creditCardsRepository = creditCardsRepository;
            _UFEService = UFEService;
            _mapper = mapper;
        }

        public async Task<PaymentResult> AddPaymentAsync(PaymentRequest paymentRequest)
        {
            var payment = _mapper.Map<Payment>(paymentRequest);

            payment.FeeAmount = Math.Round(payment.Amount * _UFEService.Fee, 2);
            payment.TotalAmount = payment.Amount + payment.FeeAmount;

            var creditCard = await _creditCardsRepository.GetCreditCardAsync(paymentRequest.CreditCardNumber);
            
            if (creditCard.AvailableCredit - payment.TotalAmount < 0)
            {
                throw new InvalidOperationException("There is not enough available credit.");
            }

            payment.CreditCardId = creditCard.Id;

            creditCard.AvailableCredit -= payment.TotalAmount;
            creditCard.TotalPayments += payment.TotalAmount;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _paymentsRepository.AddPaymentAsync(payment);

            await _creditCardsRepository.UpdateCreditCardAsync(creditCard);

            scope.Complete();

            return _mapper.Map<PaymentResult>(payment);
        }
    }
}
