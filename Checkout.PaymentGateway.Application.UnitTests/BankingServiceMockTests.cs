using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Application.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class BankingServiceMockTests
    {
        #region Creation
        private static BankingServiceMock GetService(BankingServiceMockOptions options) =>
            new BankingServiceMock(Options.Create(options));
        #endregion

        [Theory]
        [ClassData(typeof(ShouldThrowArgumentNullExceptionData))]
        public void ShouldThrowArgumentNullException(IOptions<BankingServiceMockOptions> options)
        {
            Assert.Throws<ArgumentNullException>(() => new BankingServiceMock(options));
        }

        internal class ShouldThrowArgumentNullExceptionData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null
                };
                yield return new object[]
                {
                    Options.Create<BankingServiceMockOptions>(null)
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public async void MakePaymentShouldThrowArgumentNullException()
        {
            var sut = GetService(new BankingServiceMockOptions());
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MakePaymentAsync(null));
        }

        [Fact]
        public async void PaymentShouldBeSuccessful()
        {
            const string cardNumber = "4485559617436242";
            var options = new BankingServiceMockOptions();

            var sut = GetService(options);

            var result = await sut.MakePaymentAsync(new DTO.PaymentInformation()
            {
                CardNumber = cardNumber,
                Amount = 20m,
                Currency = "EUR",
                CVV = "123",
                ExpiryYear = 2022,
                ExpityMonth = 12
            });

            Assert.True(result.Successful);
        }

        [Fact]
        public async void PaymentShouldNotBeSuccessful()
        {
            const string cardNumber = "4485559617436242";
            var options = new BankingServiceMockOptions()
            {
                InvalidCards =
                {
                    cardNumber
                }
            };

            var sut = GetService(options);

            var result = await sut.MakePaymentAsync(new DTO.PaymentInformation()
            {
                CardNumber = cardNumber,
                Amount = 20m,
                Currency = "EUR",
                CVV = "123",
                ExpiryYear = 2022,
                ExpityMonth = 12
            });

            Assert.False(result.Successful);
        }
    }
}
