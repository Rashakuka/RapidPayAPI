namespace RapidPayAPI.Repositories.Payments
{
    public interface IPaymentsRepository
    {
        Task AddPaymentAsync(Payment payment);

        Task<Payment?> GetLastestPaymentAsync();
    }
}
