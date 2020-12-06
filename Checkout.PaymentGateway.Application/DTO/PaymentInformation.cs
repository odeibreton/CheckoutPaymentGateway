using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Application.DTO
{
    public sealed class PaymentInformation
    {
        public string CardNumber { get; set; }
        public int ExpityMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
