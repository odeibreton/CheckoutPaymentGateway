using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Application.DTO
{
    public class GetPaymentByBankingPaymentIdResult
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool SuccessfulPayment { get; set; }
    }
}
