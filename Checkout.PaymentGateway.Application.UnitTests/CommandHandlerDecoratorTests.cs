using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class CommandHandlerDecoratorTests
    {

        [Fact]
        public void ShouldThrowArgumentNullExceptionWithNullHandler()
        {
            Assert.Throws<ArgumentNullException>(() => new TestDecorator<TestCommand>(null));
        }

        [Fact]
        public async void ShouldExecuteDecorator()
        {
            var handlerMock = new Mock<ICommandHandler<TestCommand>>();
            handlerMock.Setup(h => h.HandleAsync(It.IsAny<TestCommand>()))
                .Returns(Task.CompletedTask);

            bool decoratorExecuted = false;
            var sut = new TestDecorator<TestCommand>(handlerMock.Object, CallVerifyer);

            await sut.HandleAsync(new TestCommand());

            Assert.True(decoratorExecuted);

            Task CallVerifyer(TestCommand command)
            {
                decoratorExecuted = true;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async void ShouldExecuteHandler()
        {
            var handlerMock = new Mock<ICommandHandler<TestCommand>>();
            handlerMock.Setup(h => h.HandleAsync(It.IsAny<TestCommand>()))
                .Verifiable();

            var sut = new TestDecorator<TestCommand>(handlerMock.Object);

            await sut.HandleAsync(new TestCommand());

            handlerMock.Verify();
        }

        [Fact]
        public async void ShouldExecuteHandlerAndDecoratorInOrder()
        {
            Queue<string> actionQueue = new Queue<string>();

            var handlerMock = new Mock<ICommandHandler<TestCommand>>();
            handlerMock.Setup(h => h.HandleAsync(It.IsAny<TestCommand>()))
                .Callback<TestCommand>(c => actionQueue.Enqueue("Handler"))
                .Returns(Task.CompletedTask);

            var sut = new TestDecorator<TestCommand>(handlerMock.Object, DecoratorCallVerifyer);

            await sut.HandleAsync(new TestCommand());

            Assert.Equal("Decorator", actionQueue.Dequeue());
            Assert.Equal("Handler", actionQueue.Dequeue());
            Assert.Empty(actionQueue);

            Task DecoratorCallVerifyer(TestCommand command)
            {
                actionQueue.Enqueue("Decorator");
                return Task.CompletedTask;
            }
        }

        #region Setup
        public class TestCommand : ICommand
        {
        }

        internal class TestDecorator<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {
            private Func<T, Task> Expression { get; }

            public TestDecorator(ICommandHandler<T> internalHandler, Func<T, Task> expression = null) : base(internalHandler)
            {
                Expression = expression;
            }

            protected override Task HandleDecoratorAsync(T command)
            {
                if (Expression is null)
                    return Task.CompletedTask;

                return Expression.Invoke(command);
            }
        }
        #endregion
    }
}
