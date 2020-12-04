using System;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Domain.Framework.UnitTests
{
    public class ValueObjectTests
    {
        [Fact]
        public void ShouldEqual_EqualsRetunsTrue()
        {
            var mock = new Mock<ValueObject>();

            mock.Setup(vo => vo.Equals(It.IsAny<ValueObject>())).Returns(true);

            var sut = mock.Object;
            var otherValueObject = Mock.Of<ValueObject>();

            Assert.True(sut == otherValueObject);
        }

        [Fact]
        public void ShouldEqual_BothObjectsNull()
        {
            ValueObject sut = null;
            ValueObject otherValueObject = null;

            Assert.True(sut == otherValueObject);
        }

        [Fact]
        public void ShouldNotBeEqual_EqualsReturnsFalse()
        {
            var mock = new Mock<ValueObject>();

            mock.Setup(vo => vo.Equals(It.IsAny<ValueObject>())).Returns(false);

            var sut = mock.Object;
            var otherValueObject = Mock.Of<ValueObject>();

            Assert.False(sut == otherValueObject);
        }

        [Fact]
        public void ShouldNotBeEqual_LeftNull()
        {
            ValueObject sut = null;
            var otherValueObject = Mock.Of<ValueObject>();

            Assert.False(sut == otherValueObject);
        }

        [Fact]
        public void ShouldNotBeEqual_RightNull()
        {
            ValueObject sut = Mock.Of<ValueObject>(); ;
            ValueObject otherValueObject = null;

            Assert.False(sut == otherValueObject);
        }

        [Fact]
        public void ShouldNotBeEqual_NotEqualOperator()
        {
            ValueObject sut = Mock.Of<ValueObject>(); ;
            ValueObject otherValueObject = null;

            Assert.True(sut != otherValueObject);
        }
    }
}
