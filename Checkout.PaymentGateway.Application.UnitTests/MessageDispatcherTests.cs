using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class MessageDispatcherTests
    {
        private static IMessageDispatcher GetDispatcher(IServiceProvider provider) => new MessageDispatcher(provider);

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => GetDispatcher(null));
        }

        [Theory]
        [ClassData(typeof(ShouldDispatchCommandData))]
        public async void ShouldDispatchCommand(TestCommand command)
        {
            var handlerMock = new Mock<ICommandHandler<TestCommand>>();
            handlerMock.Setup(h => h.HandleAsync(It.IsAny<TestCommand>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var services = new ServiceCollection();
            services.AddTransient(provider => handlerMock.Object);

            var sut = GetDispatcher(services.BuildServiceProvider());

            await sut.DispatchAsync(command);

            handlerMock.Verify();
        }

        internal class ShouldDispatchCommandData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null };
                yield return new object[] { new TestCommand() };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ShouldDispatchQueryData))]
        public async void ShouldDispatchQuery(TestQuery query)
        {
            var handlerMock = new Mock<IQueryHandler<TestQuery, object>>();
            handlerMock.Setup(h => h.HandleAsync(It.IsAny<TestQuery>()))
                .Returns(Task.FromResult(new object()))
                .Verifiable();

            var services = new ServiceCollection();
            services.AddTransient(provider => handlerMock.Object);

            var sut = GetDispatcher(services.BuildServiceProvider());

            await sut.DispatchAsync<TestQuery, object>(query);

            handlerMock.Verify();
        }

        internal class ShouldDispatchQueryData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null };
                yield return new object[] { new TestQuery() };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #region Setup
        public class TestCommand : ICommand
        {
        }

        public class TestQuery : IQuery
        {
        }
        #endregion
    }
}
