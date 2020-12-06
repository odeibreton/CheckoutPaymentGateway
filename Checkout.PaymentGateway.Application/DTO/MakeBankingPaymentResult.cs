using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Application.DTO
{
    public class MakeBankingPaymentResult
    {
        public string Id { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
    }
}
