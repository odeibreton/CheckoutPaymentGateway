using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Bank.API.Model
{
    public class PaymentResult
    {
        public string Id { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
    }
}
