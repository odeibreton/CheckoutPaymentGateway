using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers.Abstractions
{
    public abstract class PreQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>, IDecorator
        where TQuery : IQuery
        where TResult : class
    {
        private IQueryHandler<TQuery, TResult> InternalHandler { get; }

        protected PreQueryHandlerDecorator(IQueryHandler<TQuery, TResult> internalHandler)
        {
            InternalHandler = internalHandler ?? throw new ArgumentNullException(nameof(internalHandler));
        }

        protected abstract Task HandleDecoratorAsync(TQuery query);

        public async Task<TResult> HandleAsync(TQuery query)
        {
            await HandleDecoratorAsync(query);
            return await InternalHandler.HandleAsync(query);
        }
    }
}
