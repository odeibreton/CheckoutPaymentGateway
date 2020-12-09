using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain
{
    [Serializable]
    public class InvalidCardNumberException : Exception
    {
        public InvalidCardNumberException() : this("Invalid Card Number.") { }
        public InvalidCardNumberException(string message) : base(message) { }
        public InvalidCardNumberException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCardNumberException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
