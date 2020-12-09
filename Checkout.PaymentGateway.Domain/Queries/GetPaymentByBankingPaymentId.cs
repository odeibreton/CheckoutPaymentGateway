using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain.Queries
{
    public class GetPaymentByBankingPaymentId : IQuery
    {
        public string BankingPaymentId { get; set; }
    }
}
