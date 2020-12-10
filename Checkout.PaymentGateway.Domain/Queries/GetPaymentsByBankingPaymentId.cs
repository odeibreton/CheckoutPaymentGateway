using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain.Queries
{
    public class GetPaymentsByBankingPaymentId : IQuery
    {
        public string[] Ids { get; set; }
    }
}
