using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain
{
    public sealed class PaymentId : IntId
    {
        public PaymentId(int id) : base(id)
        {
        }
    }
}
