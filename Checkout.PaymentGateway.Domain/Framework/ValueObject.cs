using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public abstract class ValueObject
    {
        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
