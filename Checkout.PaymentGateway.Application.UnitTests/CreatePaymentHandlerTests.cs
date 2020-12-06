using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Application.Services;
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
        private static CreatePaymentHandler GetHandler(IPaymentRepository repository,
                                                       IBankingService bankingService) =>
            new CreatePaymentHandler(repository, bankingService, NullLogger<CreatePaymentHandler>.Instance);

        private static (Mock<IPaymentRepository> Repository, Mock<IBankingService> Service) GetMocks() =>
            (new Mock<IPaymentRepository>(), new Mock<IBankingService>());

        private static CreatePayment GetCreatePaymentCommand() => new CreatePayment()
        {
            CardNumber = "4080231619817071",
            ExpiryYear = 2022,
            ExpityMonth = 12,
            CVV = "123",
            Amount = 200m,
            Currency = "EUR"
        };
        #endregion


        [Theory]
        [ClassData(typeof(ShouldThrowArgumentNullExceptionData))]
        public void ShouldThrowArgumentNullException(IPaymentRepository paymentRepository,
                                                     IBankingService bankingService,
                                                     ILogger<CreatePaymentHandler> logger)
        {
            Assert.Throws<ArgumentNullException>(() => new CreatePaymentHandler(paymentRepository, bankingService, logger));
        }

        internal class ShouldThrowArgumentNullExceptionData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null,
                    Mock.Of<IBankingService>(),
                    NullLogger<CreatePaymentHandler>.Instance
                };
                yield return new object[]
                {
                    Mock.Of<IPaymentRepository>(),
                    null,
                    NullLogger<CreatePaymentHandler>.Instance
                };
                yield return new object[]
                {
                    Mock.Of<IPaymentRepository>(),
                    Mock.Of<IBankingService>(),
                    null
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public async Task HandleAsyncShouldThrowArgumentNullException()
        {
            var (repository, service) = GetMocks();
            var sut = GetHandler(repository.Object, service.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.HandleAsync(null));
        }

        [Fact]
        public async Task BankingErrorShouldThrowBankingException()
        {
            var (repository, service) = GetMocks();
            service.Setup(s => s.MakePayment(It.IsAny<PaymentInformation>()))
                .ThrowsAsync(new BankingException());

            var sut = GetHandler(repository.Object, service.Object);

            await Assert.ThrowsAsync<BankingException>(async () => await sut.HandleAsync(GetCreatePaymentCommand()));
        }

        [Fact]
        public async Task RepositoryShouldNotBeCalledAfterBankingError()
        {
            var (repository, service) = GetMocks();
            service.Setup(s => s.MakePayment(It.IsAny<PaymentInformation>()))
                .ThrowsAsync(new BankingException());

            repository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var sut = GetHandler(repository.Object, service.Object);

            await Assert.ThrowsAsync<BankingException>(async () => await sut.HandleAsync(GetCreatePaymentCommand()));
            Assert.Throws<MockException>(() => repository.Verify());
        }

        [Fact]
        public async Task RepositoryCreatePaymentActionShoulBeCalledAfterSuccessfulPayment()
        {
            const string bankingPaymentId = "PAYMENT ID";

            var (repository, service) = GetMocks();
            service.Setup(s => s.MakePayment(It.IsAny<PaymentInformation>()))
                .ReturnsAsync(bankingPaymentId);

            repository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var sut = GetHandler(repository.Object, service.Object);

            await sut.HandleAsync(GetCreatePaymentCommand());

            repository.Verify();
        }

        [Fact]
        public async Task PaymentShouldBeCreatedWithBankingPaymentId()
        {
            const string expectedBankingPaymentId = "PAYMENT ID";
            string actualBankingPaymentId = null;

            var (repository, service) = GetMocks();
            service.Setup(s => s.MakePayment(It.IsAny<PaymentInformation>()))
                .ReturnsAsync(expectedBankingPaymentId);

            repository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .Callback<Payment>(p => actualBankingPaymentId = p.BankingPaymentId)
                .Returns(Task.CompletedTask);

            var sut = GetHandler(repository.Object, service.Object);

            await sut.HandleAsync(GetCreatePaymentCommand());

            Assert.Equal(expectedBankingPaymentId, actualBankingPaymentId);
        }

        [Fact]
        public async Task ShouldThrowExceptionIfRepositoryThrowsException()
        {
            const string expectedBankingPaymentId = "PAYMENT ID";

            var (repository, service) = GetMocks();
            service.Setup(s => s.MakePayment(It.IsAny<PaymentInformation>()))
                .ReturnsAsync(expectedBankingPaymentId);

            repository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .Throws(new InvalidOperationException());

            var sut = GetHandler(repository.Object, service.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.HandleAsync(GetCreatePaymentCommand()));
        }
    }
}
