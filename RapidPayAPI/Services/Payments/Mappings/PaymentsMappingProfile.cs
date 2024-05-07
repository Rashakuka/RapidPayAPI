using AutoMapper;
using RapidPayAPI.Repositories.Payments;
using RapidPayAPI.Services.Payments.Models;

namespace RapidPayAPI.Services.Payments.Mappings
{
    public class PaymentsMappingProfile : Profile
    {
        public PaymentsMappingProfile()
        {
            CreateMap<PaymentRequest, Payment>();

            CreateMap<Payment, PaymentResult>()
                .ForMember(paymentResult => paymentResult.CreditCardNumber, options =>
                    options.MapFrom(payment => payment.CreditCard.Number));
        }
    }
}
