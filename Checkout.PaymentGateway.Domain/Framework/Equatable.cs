using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public abstract class Equatable<T>
    {
        public override bool Equals([AllowNull] object obj)
        {
            if (!(obj is T other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return true;
        }

        public abstract override int GetHashCode();
    }
}
