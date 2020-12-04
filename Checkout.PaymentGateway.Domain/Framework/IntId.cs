using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public abstract class IntId : ValueObject
    {
        protected IntId(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is IntId value))
                return false;

            if (ReferenceEquals(this, value))
                return true;

            if (GetType() != value.GetType())
                return false;

            if (Id == 0 || value.Id == 0)
                return false;

            return Id == value.Id;
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id.ToString()).GetHashCode();
        }
    }
}
