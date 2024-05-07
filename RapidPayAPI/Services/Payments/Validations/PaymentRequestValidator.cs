using FluentValidation;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Services.Payments.Models;

namespace RapidPayAPI.Services.Payments.Validations
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        private readonly ICreditCardsRepository _creditCardsRepository;

        public PaymentRequestValidator(ICreditCardsRepository creditCardsRepository)
        {
            _creditCardsRepository = creditCardsRepository;

            RuleFor(paymentRequest => paymentRequest.CreditCardNumber)
                .Must(CreditCardNumberExists)
                .WithMessage("A credit card with specified number does not exist.");

            RuleFor(paymentRequest => paymentRequest.Amount)
                .Must(amount => amount > 0)
                .WithMessage("Amount cannot be less than or equal to 0.");
        }

        private bool CreditCardNumberExists(string creditNumber)
        {
            return _creditCardsRepository.CreditCardNumberExistsAsync(creditNumber).GetAwaiter().GetResult();
        }
    }
}
