using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public interface ICommandHandler<T>
        where T : ICommand
    {
        Task HandleAsync(T command);
    }

    public interface ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
        where TResult : class
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
