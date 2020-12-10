using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Bank.API.Configuration
{
    public class PaymentOptions
    {
        public List<string> InvalidCards { get; set; } = new List<string>();
        public int PaymentDelay { get; set; }
    }
}
