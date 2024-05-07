using System.ComponentModel.DataAnnotations;

namespace RapidPayAPI.Services.Payments.Models
{
    public class PaymentRequest
    {
        [Required]
        [StringLength(15, ErrorMessage = "Credit card number must be 15 characteres")]
        [CreditCard]
        public string CreditCardNumber { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }
    }
}
