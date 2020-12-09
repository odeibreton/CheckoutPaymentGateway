using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain
{
    public sealed class Currency : ValueObject
    {
        public string Value { get; private set; }

        public Currency(string value)
        {
            _ = value ?? throw new ArgumentNullException("currency");

            Validate(value);

            Value = value;
        }

        private void Validate(string value)
        {
            if (value.Length != 3)
            {
                throw new InvalidCurrencyException();
            }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as Currency;

            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return $"{GetType()} {Value}".GetHashCode();
        }
    }
}
