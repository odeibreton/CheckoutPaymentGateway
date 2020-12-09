using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Commands;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public abstract class EncryptDecorator<T> : CommandHandlerDecorator<T>
        where T : ICommand
    {
        private readonly IEncryptionService _encryptionService;

        protected EncryptDecorator(ICommandHandler<T> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        protected string Encrypt(string value)
        {
            return _encryptionService.Encrypt(value);
        }
    }

    public abstract class PreEncryptDecorator<TQuery, TResult> : PreQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery
        where TResult : class
    {
        private readonly IEncryptionService _encryptionService;

        protected PreEncryptDecorator(IQueryHandler<TQuery, TResult> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        protected string Encrypt(string value)
        {
            return _encryptionService.Encrypt(value);
        }
    }

    public abstract class PostEncryptDecorator<TQuery, TResult> : PostQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery
        where TResult : class
    {
        private readonly IEncryptionService _encryptionService;

        protected PostEncryptDecorator(IQueryHandler<TQuery, TResult> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        protected string Encrypt(string value)
        {
            return _encryptionService.Encrypt(value);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EncryptAttribute : HandlerAttribute
    {
        public EncryptAttribute(Type commandType)
            : base(typeof(EncryptDecorator<>), commandType)
        {
        }

        public EncryptAttribute(Type queryType, Type queryResultType, DecoratorExecutionTime executionTime)
            : base(executionTime == DecoratorExecutionTime.Pre ? typeof(PreEncryptDecorator<,>) : typeof(PostEncryptDecorator<,>), queryType, queryResultType, executionTime)
        {
        }
    }
}
