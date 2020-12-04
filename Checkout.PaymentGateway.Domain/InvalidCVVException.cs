using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain
{

    [Serializable]
    public class InvalidCVVException : Exception
    {
        public InvalidCVVException() : this("Invalid CVV.") { }
        public InvalidCVVException(string message) : base(message) { }
        public InvalidCVVException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCVVException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
