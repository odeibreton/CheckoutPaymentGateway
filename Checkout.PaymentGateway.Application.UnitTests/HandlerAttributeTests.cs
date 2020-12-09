using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class HandlerAttributeTests
    {
        [Fact]
        public void ShouldLocateDecorator()
        {
            var sut = new EncryptAttribute(typeof(DoAction));
            Assert.Equal(typeof(EncryptDecorator<DoAction>), sut.Decorator);
        }

        #region Setup
        internal class DoAction : ICommand
        {
        }

        internal class EncryptAttribute : HandlerAttribute
        {
            public EncryptAttribute(Type commandType) : base(typeof(EncryptDecorator<>), commandType)
            {
            }
        }

        internal class DecryptAttribute : HandlerAttribute
        {
            public DecryptAttribute(Type commandType) : base(typeof(DecryptDecorator<>), commandType)
            {
            }
        }

        internal abstract class EncryptDecorator<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {
            public EncryptDecorator(ICommandHandler<T> internalHandler) : base(internalHandler)
            {
            }
        }

        internal abstract class DecryptDecorator<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {
            public DecryptDecorator(ICommandHandler<T> internalHandler) : base(internalHandler)
            {
            }
        }

        internal class EncryptDoActionDecorator : EncryptDecorator<DoAction>
        {
            public EncryptDoActionDecorator(ICommandHandler<DoAction> internalHandler) : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(DoAction command)
            {
                throw new NotImplementedException();
            }
        }

        internal class DecryptDoActionDecorator : DecryptDecorator<DoAction>
        {
            public DecryptDoActionDecorator(ICommandHandler<DoAction> internalHandler) : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(DoAction command)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
