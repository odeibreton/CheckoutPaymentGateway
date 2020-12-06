using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Domain.Framework;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class CommandHandlerAttributeTests
    {
        [Fact]
        public void ShouldReturnDecorator()
        {
            var sut = new TestAttribute();
            Assert.Equal(typeof(TestDecorator), sut.Decorator);
        }

        #region Setup
        internal class TestCommand : ICommand
        {
        }

        internal class TestAttribute : CommandHandlerAttribute
        {
            public override Type Decorator => typeof(TestDecorator);
        }

        internal class TestDecorator : CommandHandlerDecorator<TestCommand>
        {
            public TestDecorator(ICommandHandler<TestCommand> internalHandler) : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(TestCommand command)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
