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
            if (!base.Equals(obj))
                return false;

            var value = obj as IntId;

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
