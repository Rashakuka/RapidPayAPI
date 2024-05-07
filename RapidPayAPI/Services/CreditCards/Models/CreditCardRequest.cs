using System.ComponentModel.DataAnnotations;

namespace RapidPayAPI.Services.CreditCards.Models
{
    public class CreditCardRequest
    {
        [Required]
        [StringLength(15, ErrorMessage = "Credit card number must be 15 characteres")]
        [CreditCard]
        public string Number { get; set; } = string.Empty;
    }
}
