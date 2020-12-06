using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public abstract class CommandHandlerDecorator<T> : ICommandHandler<T>
        where T : ICommand
    {
        private ICommandHandler<T> InternalHandler { get; }

        protected CommandHandlerDecorator(ICommandHandler<T> internalHandler)
        {
            InternalHandler = internalHandler ?? throw new ArgumentNullException(nameof(internalHandler));
        }

        protected abstract Task HandleDecoratorAsync(T command);

        public async Task HandleAsync(T command)
        {
            await HandleDecoratorAsync(command);
            await InternalHandler.HandleAsync(command);
        }
    }
}
