using FluentValidation;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Services.CreditCards.Models;

namespace RapidPayAPI.Services.CreditCards.Validations
{
    public class CardBalanceRequestValidator : AbstractValidator<CardBalanceRequest>
    {
        private readonly ICreditCardsRepository _creditCardsRepository;

        public CardBalanceRequestValidator(ICreditCardsRepository creditCardsRepository)
        {
            _creditCardsRepository = creditCardsRepository;

            RuleFor(creditCardInput => creditCardInput.Number)
                .Must(CreditCardNumberExists)
                .WithMessage("There is no credit card with a specified number.");
        }

        private bool CreditCardNumberExists(string creditNumber)
        {
            return _creditCardsRepository.CreditCardNumberExistsAsync(creditNumber).GetAwaiter().GetResult();
        }
    }
}
