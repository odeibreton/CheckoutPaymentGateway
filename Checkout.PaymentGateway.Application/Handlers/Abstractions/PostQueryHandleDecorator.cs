using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers.Abstractions
{
    public abstract class PostQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>, IDecorator
        where TQuery : IQuery
        where TResult : class
    {
        private IQueryHandler<TQuery, TResult> InternalHandler { get; }

        protected PostQueryHandlerDecorator(IQueryHandler<TQuery, TResult> internalHandler)
        {
            InternalHandler = internalHandler ?? throw new ArgumentNullException(nameof(internalHandler));
        }

        protected abstract Task<TResult> HandleDecoratorAsync(TQuery query, TResult result);

        public async Task<TResult> HandleAsync(TQuery query)
        {
            var result = await InternalHandler.HandleAsync(query);
            result = await HandleDecoratorAsync(query, result);
            return result;
        }
    }
}
