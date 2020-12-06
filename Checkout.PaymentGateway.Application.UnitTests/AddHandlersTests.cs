using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class AddHandlersTests
    {
        private static IServiceCollection GetServices() => new ServiceCollection();

        [Fact]
        public void NullAssemblyShouldThrowException()
        {
            var services = GetServices();
            Assert.Throws<ArgumentNullException>(() => services.AddHandlers(null));
        }

        [Fact]
        public void ShouldRegisterSimpleHandler()
        {
            var services = GetServices();
            services.AddHandlers(typeof(AddHandlersTests).Assembly);

            var handler = services.BuildServiceProvider().GetService<ICommandHandler<ExecuteSimpleAction>>();

            Assert.NotNull(handler);
            Assert.IsType<SimpleHandler>(handler);
        }

        [Fact]
        public void ShouldRegisterComplexHandler()
        {
            var services = GetServices();
            services.AddHandlers(typeof(AddHandlersTests).Assembly);

            var handler = services.BuildServiceProvider().GetService<ICommandHandler<ExecuteComplexAction>>();

            Assert.NotNull(handler);
        }

        [Fact]
        public void ShouldRegisterComplexHandlerWithDecorators()
        {
            var services = GetServices();
            services.AddHandlers(typeof(AddHandlersTests).Assembly);

            var handler = services.BuildServiceProvider().GetService<ICommandHandler<ExecuteComplexAction>>();

            Assert.NotNull(handler);

            var command = new ExecuteComplexAction();
            handler.HandleAsync(command);

            Assert.Equal("Decorator1", command.History.Dequeue());
            Assert.Equal("Decorator2", command.History.Dequeue());
            Assert.Equal("Handler", command.History.Dequeue());
            Assert.Empty(command.History);
        }

        #region Setup

        internal class ExecuteSimpleAction : ICommand
        {
        }

        internal class SimpleHandler : ICommandHandler<ExecuteSimpleAction>
        {
            public Task HandleAsync(ExecuteSimpleAction command)
            {
                throw new NotImplementedException();
            }
        }

        internal class ExecuteComplexAction : ICommand
        {
            public Queue<string> History { get; set; } = new Queue<string>();
        }

        [Example1]
        [Example2]
        internal class ComplexHandler : ICommandHandler<ExecuteComplexAction>
        {
            public Task HandleAsync(ExecuteComplexAction command)
            {
                command.History.Enqueue("Handler");
                return Task.CompletedTask;
            }
        }

        internal class Example1Attribute : CommandHandlerAttribute
        {
            public override Type Decorator => typeof(Example1Decorator<>);
        }

        internal class Example2Attribute : CommandHandlerAttribute
        {
            public override Type Decorator => typeof(Example2Decorator<>);
        }

        internal class Example1Decorator<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {
            public Example1Decorator(ICommandHandler<T> internalHandler) : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(T command)
            {
                (command as ExecuteComplexAction).History.Enqueue("Decorator1");
                return Task.CompletedTask;
            }
        }

        internal class Example2Decorator<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {
            public Example2Decorator(ICommandHandler<T> internalHandler) : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(T command)
            {
                (command as ExecuteComplexAction).History.Enqueue("Decorator2");
                return Task.CompletedTask;
            }
        }

        #endregion
    }
}
