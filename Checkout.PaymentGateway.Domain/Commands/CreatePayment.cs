using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain.Commands
{
    public class CreatePayment : ICommand<PaymentId>
    {
        public string CardNumber { get; set; }
        public int ExpityMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
