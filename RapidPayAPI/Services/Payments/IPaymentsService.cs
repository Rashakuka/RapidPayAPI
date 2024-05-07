using RapidPayAPI.Repositories.Payments;
using RapidPayAPI.Services.Payments.Models;

namespace RapidPayAPI.Services.Payments
{
    public interface IPaymentsService
    {
        Task<PaymentResult> AddPaymentAsync(PaymentRequest paymentRequest);
    }
}
