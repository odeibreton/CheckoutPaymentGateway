using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Application.Handlers.CreatePayment;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain;
using Checkout.PaymentGateway.Domain.Commands;
using Checkout.PaymentGateway.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class CreatePaymentHandlerTests
    {
        #region Creation
        private static Handler GetHandler(IPaymentRepository repository) =>
            new Handler(repository, NullLogger<Handler>.Instance);

        private static Mock<IPaymentRepository> GetMock() => new Mock<IPaymentRepository>();
        #endregion


        [Theory]
        [ClassData(typeof(ShouldThrowArgumentNullExceptionData))]
        public void ShouldThrowArgumentNullException(IPaymentRepository paymentRepository,
                                                     ILogger<Handler> logger)
        {
            Assert.Throws<ArgumentNullException>(() => new Handler(paymentRepository, logger));
        }

        internal class ShouldThrowArgumentNullExceptionData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null,
                    NullLogger<Handler>.Instance
                };
                yield return new object[]
                {
                    Mock.Of<IPaymentRepository>(),
                    null
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public async Task HandleAsyncShouldThrowArgumentNullException()
        {
            var repository = GetMock();
            var sut = GetHandler(repository.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.HandleAsync(null));
        }

        [Fact]
        public async Task ShouldThrowExceptionIfRepositoryThrowsException()
        {
            var command = new CreatePayment()
            {
                BankingPaymentId = "PAYMENT ID",
                Successful = true,
                CardNumber = "4080231619817071",
                ExpiryYear = 2022,
                ExpiryMonth = 12,
                CVV = "123",
                Amount = 200m,
                Currency = "EUR"
            };

            var repository = GetMock();

            repository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .Throws(new InvalidOperationException());

            var sut = GetHandler(repository.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.HandleAsync(command));
        }

        [Fact]
        public async Task ShouldAddAndSavePayment()
        {
            var command = new CreatePayment()
            {
                BankingPaymentId = "PAYMENT ID",
                Successful = true,
                CardNumber = "4080231619817071",
                ExpiryYear = 2022,
                ExpiryMonth = 12,
                CVV = "123",
                Amount = 200m,
                Currency = "EUR"
            };

            var repository = GetMock();
            repository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            repository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            var sut = GetHandler(repository.Object);

            await sut.HandleAsync(command);

            repository.Verify();
        }
    }
}
