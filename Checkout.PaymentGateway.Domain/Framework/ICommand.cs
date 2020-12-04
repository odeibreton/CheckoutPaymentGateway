using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public interface ICommand<TResult> where TResult : class
    {
    }

    public interface ICommand
    {
    }
}
