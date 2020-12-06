using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain
{
    public sealed class CVV : ValueObject
    {
        public string Value { get; private set; }

        public CVV(string cvv)
        {
            _ = cvv ?? throw new ArgumentNullException(nameof(cvv));

            Validate(cvv);

            Value = cvv;
        }

        private void Validate(string value)
        {
            if (value.Length != 3 && value.Length != 4)
            {
                throw new InvalidCVVException();
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as CVV;

            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return $"{GetType()} {Value}".GetHashCode();
        }
    }
}
