using AutoMapper;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Services.CreditCards.Models;

namespace RapidPayAPI.Services.CreditCards.Mappings
{
    public class CreditCardsMappingProfile : Profile
    {
        public CreditCardsMappingProfile()
        {
            CreateMap<CreditCardRequest, CreditCard>();

            CreateMap<CreditCard, CreditCardResult>();

            CreateMap<CreditCard, CardBalanceResult>();
        }
    }
}
