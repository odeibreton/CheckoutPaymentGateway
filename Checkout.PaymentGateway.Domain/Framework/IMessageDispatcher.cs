using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public interface IMessageDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
            where TResult : class;
    }
}
