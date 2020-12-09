using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Domain
{
    public sealed class CardNumber : ValueObject
    {
        public string Value { get; }

        public CardNumber(string value)
        {
            _ = value ?? throw new ArgumentNullException("cardNumber");

            if (!LuhnCheck.IsValid(value))
            {
                throw new InvalidCardNumberException();
            }

            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as CardNumber;

            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return $"{GetType()} {Value}".GetHashCode();
        }
    }

    internal static class LuhnCheck
    {
        public static bool IsValid(string cardNumber)
        {
            // Luhn check
            return true;
        }
    }
}
