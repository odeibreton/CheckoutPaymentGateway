using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain
{

    [Serializable]
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException() : this("Invalid currency.") { }
        public InvalidCurrencyException(string message) : base(message) { }
        public InvalidCurrencyException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCurrencyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
