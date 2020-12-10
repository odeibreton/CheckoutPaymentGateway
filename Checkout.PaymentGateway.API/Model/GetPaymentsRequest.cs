using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.API.Model
{
    public class GetPaymentsRequest
    {
        [Required]
        public string[] Ids { get; set; }
    }
}
