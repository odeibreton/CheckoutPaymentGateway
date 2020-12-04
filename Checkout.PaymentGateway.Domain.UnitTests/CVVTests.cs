using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Checkout.PaymentGateway.Domain.UnitTests
{
    public class CVVTests
    {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CVV(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123456")]
        public void ShouldThrowInvalidCVVException(string cvv)
        {
            Assert.Throws<InvalidCVVException>(() => new CVV(cvv));
        }

        [Theory]
        [InlineData("111")]
        [InlineData("123")]
        [InlineData("1111")]
        [InlineData("1234")]
        public void ShouldEqual(string cvv)
        {
            var sut = new CVV(cvv);
            var other = new CVV(cvv);

            Assert.Equal(sut, other);
        }

        [Theory]
        [InlineData("111", "123")]
        [InlineData("123", "1234")]
        [InlineData("1111", "111")]
        [InlineData("1234", "123")]
        public void ShouldNotEqual(string cvv1, string cvv2)
        {
            var sut = new CVV(cvv1);
            var other = new CVV(cvv2);

            Assert.NotEqual(sut, other);
        }
    }
}
