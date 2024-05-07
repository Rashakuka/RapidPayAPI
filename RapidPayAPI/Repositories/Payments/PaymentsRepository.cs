using Microsoft.EntityFrameworkCore;
using RapidPayAPI.Context;

namespace RapidPayAPI.Repositories.Payments
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly RapidPayAPIDbContext _rapidPayDbContext;

        public PaymentsRepository(RapidPayAPIDbContext rapidPayDbContext)
        {
            _rapidPayDbContext = rapidPayDbContext;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            _rapidPayDbContext.Payments.Add(payment);
            await _rapidPayDbContext.SaveChangesAsync();

            await _rapidPayDbContext.Entry(payment)
                .Reference(payment => payment.CreditCard).LoadAsync();
        }

        public async Task<Payment?> GetLastestPaymentAsync()
        {
            return await _rapidPayDbContext.Payments
                .OrderByDescending(payment => payment.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
