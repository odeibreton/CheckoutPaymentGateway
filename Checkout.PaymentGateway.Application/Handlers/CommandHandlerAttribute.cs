using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Application.Handlers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public abstract class CommandHandlerAttribute : Attribute
    {
        public abstract Type Decorator { get; }
    }
}
