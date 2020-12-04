using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Checkout.PaymentGateway.Domain.UnitTests
{
    public class CurrencyTests
    {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Currency(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("EUR ")]
        [InlineData("USDQ")]
        public void ShouldThrowInvalidCVVException(string currency)
        {
            Assert.Throws<InvalidCurrencyException>(() => new Currency(currency));
        }

        [Theory]
        [InlineData("EUR")]
        [InlineData("USD")]
        public void ShouldEqual(string currency)
        {
            var sut = new Currency(currency);
            var other = new Currency(currency);

            Assert.Equal(sut, other);
        }

        [Theory]
        [InlineData("EUR", "USD")]
        public void ShouldNotEqual(string currency1, string currency2)
        {
            var sut = new Currency(currency1);
            var other = new Currency(currency2);

            Assert.NotEqual(sut, other);
        }
    }
}
