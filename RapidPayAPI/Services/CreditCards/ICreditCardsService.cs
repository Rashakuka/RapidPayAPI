using RapidPayAPI.Services.CreditCards.Models;

namespace RapidPayAPI.Services.CreditCards
{
    public interface ICreditCardsService
    {
        Task<CreditCardResult> AddCreditCardAsync(CreditCardRequest creditCardInput);

        Task<CardBalanceResult> GetCardBalanceAsync(CardBalanceRequest cardBalanceInput);
    }
}
