using Microsoft.EntityFrameworkCore;
using RapidPayAPI.Context;

namespace RapidPayAPI.Repositories.Cards
{
    public class CreditCardsRepository : ICreditCardsRepository
    {
        private readonly RapidPayAPIDbContext _rapidPayAPIDbContext;

        public CreditCardsRepository(RapidPayAPIDbContext rapidPayDbContext)
        {
            _rapidPayAPIDbContext = rapidPayDbContext;
        }

        public async Task AddCreditCardAsync(CreditCard creditCard)
        {
            _rapidPayAPIDbContext.CreditCards.Add(creditCard);
            await _rapidPayAPIDbContext.SaveChangesAsync();
        }

        public async Task<bool> CreditCardNumberExistsAsync(string number)
        {
            return await _rapidPayAPIDbContext.CreditCards
                .Where(creditCard => creditCard.Number == number).AnyAsync();
        }

        public async Task<CreditCard?> GetCreditCardAsync(string number)
        {
            return await _rapidPayAPIDbContext.CreditCards
                .FirstOrDefaultAsync(creditCard => creditCard.Number == number);
        }

        public async Task UpdateCreditCardAsync(CreditCard creditCard)
        {
            _rapidPayAPIDbContext.Update(creditCard);
            await _rapidPayAPIDbContext.SaveChangesAsync();
        }
    }
}
