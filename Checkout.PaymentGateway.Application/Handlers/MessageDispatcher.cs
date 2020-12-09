using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public sealed class MessageDispatcher : IMessageDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task DispatchAsync<T>(T command) where T : ICommand
        {
            var handler = _serviceProvider.GetService<ICommandHandler<T>>();
            return handler.HandleAsync(command);
        }

        public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TResult : class
            where TQuery : IQuery
        {
            var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();
            return handler.HandleAsync(query);
        }
    }
}
