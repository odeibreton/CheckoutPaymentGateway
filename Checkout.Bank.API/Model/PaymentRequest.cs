using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Bank.API.Model
{
    public class PaymentRequest
    {
        [CreditCard]
        [Required]
        public string CardNumber { get; set; }
        [Required]
        [Range(1, 12)]
        public int? ExpiryMonth { get; set; }
        [Required]
        public int? ExpiryYear { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }
    }
}
