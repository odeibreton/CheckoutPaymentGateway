using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Collections;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;
using Checkout.PaymentGateway.Application.DTO;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Domain.Commands;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class CreatePaymentServiceTests
    {
        #region Creation
        private static CreatePaymentService GetService(IMessageDispatcher dispatcher, IBankingService bankingService) =>
            new CreatePaymentService(dispatcher, bankingService, NullLogger<CreatePaymentService>.Instance);

        private static (Mock<IMessageDispatcher> Dispatcher, Mock<IBankingService> BankingService) GetMocks() =>
            (new Mock<IMessageDispatcher>(), new Mock<IBankingService>());
        #endregion

        [Theory]
        [ClassData(typeof(ShouldThrowArgumentNullExceptionData))]
        public void ShouldThrowArgumentNullException(IMessageDispatcher dispatcher,
                                                     IBankingService bankingService,
                                                     ILogger<CreatePaymentService> logger)
        {
            Assert.Throws<ArgumentNullException>(() => new CreatePaymentService(dispatcher, bankingService, logger));
        }

        public class ShouldThrowArgumentNullExceptionData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null,
                    Mock.Of<IBankingService>(),
                    NullLogger<CreatePaymentService>.Instance
                };
                yield return new object[]
                {
                    Mock.Of<IMessageDispatcher>(),
                    null,
                    NullLogger<CreatePaymentService>.Instance
                };
                yield return new object[]
                {
                    Mock.Of<IMessageDispatcher>(),
                    Mock.Of<IBankingService>(),
                    null
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public async void MakePaymentShouldThrowArgumentNullException()
        {
            var (dispatcher, bankingService) = GetMocks();
            var sut = GetService(dispatcher.Object, bankingService.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MakePaymentAsync(null));
        }

        [Fact]
        public async void MakePaymentShouldCallBankingService()
        {
            var (dispatcher, bankingService) = GetMocks();
            bankingService.Setup(s => s.MakePaymentAsync(It.IsAny<PaymentInformation>()))
                .Returns(Task.FromResult(new BankingPaymentResult()))
                .Verifiable();

            var sut = GetService(dispatcher.Object, bankingService.Object);

            await sut.MakePaymentAsync(new PaymentInformation());

            bankingService.Verify();
        }

        [Fact]
        public async void BankingErrorShouldThrowBankingException()
        {
            var (dispatcher, bankingService) = GetMocks();
            bankingService.Setup(s => s.MakePaymentAsync(It.IsAny<PaymentInformation>()))
                .ThrowsAsync(new Exception());

            var sut = GetService(dispatcher.Object, bankingService.Object);

            await Assert.ThrowsAsync<BankingException>(() => sut.MakePaymentAsync(new PaymentInformation()));
        }

        [Fact]
        public async void ShouldDispatchCommandWithCorrectBankingPaymentDetails()
        {
            var paymentResult = new BankingPaymentResult()
            {
                Id = "ID",
                Successful = true
            };

            CreatePayment dispatched = null;

            var (dispatcher, bankingService) = GetMocks();
            bankingService.Setup(s => s.MakePaymentAsync(It.IsAny<PaymentInformation>()))
                .Returns(Task.FromResult(paymentResult));

            dispatcher.Setup(d => d.DispatchAsync(It.IsAny<CreatePayment>()))
                .Callback<CreatePayment>(command => dispatched = command);

            var sut = GetService(dispatcher.Object, bankingService.Object);

            await sut.MakePaymentAsync(new PaymentInformation());

            Assert.Equal(paymentResult.Id, dispatched.BankingPaymentId);
        }
    }
}
