using System;
using Xunit;

namespace Checkout.PaymentGateway.Domain.UnitTests
{
    public class CardNumberTests
    {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CardNumber(null));
        }

        [Fact]
        public void ShouldEqual()
        {
            var sut = new CardNumber("4556972104234182");
            var other = new CardNumber("4556972104234182");

            Assert.Equal(sut, other);
        }


        [Fact]
        public void ShouldNotEqual()
        {
            var sut = new CardNumber("4556972104234182");
            var other = new CardNumber("4716252010582637");

            Assert.NotEqual(sut, other);
        }
    }
}
