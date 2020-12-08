using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.API.Model
{
    public class MakePaymentResult
    {
        public string Id { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
    }
}
