using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class AddHandlersCommandTests
    {
        #region Creation
        private static IServiceCollection GetServices() => new ServiceCollection();
        private static Type[] GetTypes() => typeof(AddHandlersCommandTests).GetNestedTypes().Except(new List<Type>() { typeof(Action3Handler) }).ToArray();
        #endregion

        [Fact]
        public void ShouldRegisterHandlers()
        {
            var services = GetServices();
            services.AddHandlers(GetTypes());
            services.AddTransient(provider => "");

            var provider = services.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ICommandHandler<DoAction1>>());
            Assert.NotNull(provider.GetService<ICommandHandler<DoAction2>>());
        }

        [Fact]
        public async void ShouldRegisterAction1DecoratorsInOrder()
        {
            var services = GetServices();
            services.AddHandlers(GetTypes());
            var provider = services.BuildServiceProvider();

            var handler = provider.GetService<ICommandHandler<DoAction1>>();
            var command = new DoAction1();

            Assert.NotNull(handler);
            await handler.HandleAsync(command);
            Assert.Equal("Action1Encrypt", command.History.Dequeue());
            Assert.Equal("Action1Decrypt", command.History.Dequeue());
            Assert.Equal("Action1Handler", command.History.Dequeue());
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWithInvalidParameter()
        {
            var services = GetServices();
            services.AddHandlers(GetTypes());
            var provider = services.BuildServiceProvider();

            Assert.Throws<ArgumentException>(() => provider.GetService<ICommandHandler<DoAction2>>());
        }

        [Fact]
        public void ShouldThrowExceptionIfDecoratoCannotBeResolved()
        {
            var services = GetServices();
            Assert.ThrowsAny<Exception>(() => services.AddHandlers(typeof(AddHandlersCommandTests).GetNestedTypes()));
        }

        #region Setup
        public class DoAction1 : ICommand
        {
            public Queue<string> History { get; private set; } = new Queue<string>();
        }

        public class DoAction2 : ICommand
        {
            public Queue<string> History { get; private set; } = new Queue<string>();
        }

        public class DoAction3 : ICommand
        {
            public Queue<string> History { get; private set; } = new Queue<string>();
        }

        [Encrypt(typeof(DoAction1))]
        [Decrypt(typeof(DoAction1))]
        public class Action1Handler : ICommandHandler<DoAction1>
        {
            public Action1Handler()
            {
            }

            public Task HandleAsync(DoAction1 command)
            {
                command.History.Enqueue("Action1Handler");
                return Task.CompletedTask;
            }
        }

        [Decrypt(typeof(DoAction2))]
        [Encrypt(typeof(DoAction2))]
        public class Action2Handler : ICommandHandler<DoAction2>
        {
            public Action2Handler()
            {
            }

            public Task HandleAsync(DoAction2 command)
            {
                command.History.Enqueue("Action2Handler");
                return Task.CompletedTask;
            }
        }

        [Decrypt(typeof(DoAction3))]
        public class Action3Handler : ICommandHandler<DoAction3>
        {
            public Action3Handler()
            {
            }

            public Task HandleAsync(DoAction3 command)
            {
                command.History.Enqueue("Action3Handler");
                return Task.CompletedTask;
            }
        }

        public class EncryptAction1 : Encrypt<DoAction1>
        {

            public EncryptAction1(ICommandHandler<DoAction1> internalHandler)
                : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(DoAction1 command)
            {
                command.History.Enqueue("Action1Encrypt");
                return Task.CompletedTask;
            }
        }

        public class EncryptAction2 : Encrypt<DoAction2>
        {

            public EncryptAction2(ICommandHandler<DoAction2> internalHandler)
                : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(DoAction2 command)
            {
                command.History.Enqueue("Action2Encrypt");
                return Task.CompletedTask;
            }
        }

        public abstract class Encrypt<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {

            public Encrypt(ICommandHandler<T> internalHandler)
                : base(internalHandler)
            {
            }

            protected abstract override Task HandleDecoratorAsync(T command);
        }

        public class DecryptAction1 : Decrypt<DoAction1>
        {

            public DecryptAction1(ICommandHandler<DoAction1> internalHandler)
                : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(DoAction1 command)
            {
                command.History.Enqueue("Action1Decrypt");
                return Task.CompletedTask;
            }
        }

        public class DecryptAction2 : Decrypt<DoAction2>
        {

            public DecryptAction2(ICommandHandler<DoAction2> internalHandler, string invalidParameter)
                : base(internalHandler)
            {
            }

            protected override Task HandleDecoratorAsync(DoAction2 command)
            {
                command.History.Enqueue("Action2Decrypt");
                return Task.CompletedTask;
            }
        }

        public abstract class Decrypt<T> : CommandHandlerDecorator<T>
            where T : ICommand
        {

            public Decrypt(ICommandHandler<T> internalHandler)
                : base(internalHandler)
            {
            }

            protected abstract override Task HandleDecoratorAsync(T command);
        }

        public class EncryptAttribute : HandlerAttribute
        {
            public EncryptAttribute(Type commandType) : base(typeof(Encrypt<>), commandType)
            {
            }
        }

        public class DecryptAttribute : HandlerAttribute
        {
            public DecryptAttribute(Type commandType) : base(typeof(Decrypt<>), commandType)
            {
            }
        }
        #endregion
    }
}
