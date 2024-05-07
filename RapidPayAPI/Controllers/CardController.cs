using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPayAPI.Services.CreditCards;
using RapidPayAPI.Services.CreditCards.Models;
using RapidPayAPI.Services.Payments;
using RapidPayAPI.Services.Payments.Models;
using System.Net;

namespace RapidPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICreditCardsService _creditCardsService;

        private readonly IPaymentsService _paymentsService;

        public CardController(ICreditCardsService creditCardsService, IPaymentsService paymentsService)
        {
            _creditCardsService = creditCardsService;
            _paymentsService = paymentsService;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(CreditCardResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCreditCard([FromBody] CreditCardRequest creditCardInput)
        {
            return Ok(await _creditCardsService.AddCreditCardAsync(creditCardInput));
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(CardBalanceRequest), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet("balance")]
        public async Task<IActionResult> GetCardBalance([FromQuery] CardBalanceRequest cardBalanceRequest)
        {
            return Ok(await _creditCardsService.GetCardBalanceAsync(cardBalanceRequest));
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(PaymentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("pay")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentRequest paymentRequest)
        {
            return Ok(await _paymentsService.AddPaymentAsync(paymentRequest));
        }
    }
}
