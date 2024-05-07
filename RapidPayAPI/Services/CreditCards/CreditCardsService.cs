using AutoMapper;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Services.CreditCards.Models;
using System.Transactions;

namespace RapidPayAPI.Services.CreditCards
{
    public class CreditCardsService : ICreditCardsService
    {
        private readonly ICreditCardsRepository _creditCardsRepository;

        private readonly IMapper _mapper;

        public CreditCardsService(ICreditCardsRepository creditCardsRepository, IMapper mapper)
        {
            _creditCardsRepository = creditCardsRepository;
            _mapper = mapper;
        }

        public async Task<CreditCardResult> AddCreditCardAsync(CreditCardRequest creditCardInput)
        {
            var creditCard = _mapper.Map<CreditCard>(creditCardInput);

            creditCard.CreditLimit = Constants.LineOfCredit;
            creditCard.AvailableCredit = Constants.LineOfCredit;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _creditCardsRepository.AddCreditCardAsync(creditCard);

            scope.Complete();

            return _mapper.Map<CreditCardResult>(creditCard);
        }

        public async Task<CardBalanceResult> GetCardBalanceAsync(CardBalanceRequest cardBalanceInput)
        {
            var creditCard = await _creditCardsRepository.GetCreditCardAsync(cardBalanceInput.Number);

            return _mapper.Map<CardBalanceResult>(creditCard);
        }
    }
}
