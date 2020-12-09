using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Application
{
    [Serializable]
    public class BankingException : Exception
    {
        private const string defaultErrorMessage = "A banking related process could not be completed due to an error.";

        public BankingException() : this(defaultErrorMessage) { }
        public BankingException(string message) : base(message) { }
        public BankingException(Exception inner) : base(defaultErrorMessage, inner) { }
        public BankingException(string message, Exception inner) : base(message, inner) { }
        protected BankingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
