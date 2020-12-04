using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public abstract class AggregateRoot<T> : Entity<T>
        where T : ValueObject
    {
        public AggregateRoot(T id)
            : base(id)
        {

        }
    }
}
