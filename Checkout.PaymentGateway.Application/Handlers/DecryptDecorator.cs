using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Commands;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public abstract class DecryptDecorator<T> : CommandHandlerDecorator<T>
        where T : ICommand
    {
        private readonly IEncryptionService _encryptionService;

        protected DecryptDecorator(ICommandHandler<T> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        protected string Decrypt(string value)
        {
            return _encryptionService.Decrypt(value);
        }
    }

    public abstract class PreDecryptDecorator<TQuery, TResult> : PreQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery
        where TResult : class
    {
        private readonly IEncryptionService _encryptionService;

        protected PreDecryptDecorator(IQueryHandler<TQuery, TResult> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        protected string Decrypt(string value)
        {
            return _encryptionService.Decrypt(value);
        }
    }

    public abstract class PostDecryptDecorator<TQuery, TResult> : PostQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery
        where TResult : class
    {
        private readonly IEncryptionService _encryptionService;

        protected PostDecryptDecorator(IQueryHandler<TQuery, TResult> internalHandler, IEncryptionService encryptionService)
            : base(internalHandler)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        protected string Decrypt(string value)
        {
            return _encryptionService.Decrypt(value);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DecryptAttribute : HandlerAttribute
    {
        public DecryptAttribute(Type commandType)
            : base(typeof(DecryptDecorator<>), commandType)
        {
        }

        public DecryptAttribute(Type queryType, Type queryResultType, DecoratorExecutionTime executionTime)
            : base(executionTime == DecoratorExecutionTime.Pre ? typeof(PreDecryptDecorator<,>) : typeof(PostDecryptDecorator<,>), queryType, queryResultType, executionTime)
        {
        }
    }
}
