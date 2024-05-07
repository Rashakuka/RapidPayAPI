using FluentValidation;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Services.CreditCards.Models;

namespace RapidPayAPI.Services.CreditCards.Validations
{
    public class CreditCardRequestValidator : AbstractValidator<CreditCardRequest>
    {
        private readonly ICreditCardsRepository _creditCardsRepository;

        public CreditCardRequestValidator(ICreditCardsRepository creditCardsRepository)
        {
            _creditCardsRepository = creditCardsRepository;

            RuleFor(creditCardRequest => creditCardRequest.Number)
                .Must(CreditCardNumberIsNew)
                .WithMessage("There is already another card with the same number.");
        }

        private bool CreditCardNumberIsNew(string creditNumber)
        {
            return !_creditCardsRepository.CreditCardNumberExistsAsync(creditNumber).GetAwaiter().GetResult();
        }
    }
}
