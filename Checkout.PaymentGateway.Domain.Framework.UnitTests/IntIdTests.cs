using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Domain.Framework.UnitTests
{
    public class IntIdTests
    {
        private static Mock<IntId> GetMock(int id) => new Mock<IntId>(id)
        {
            CallBase = true
        };

        [Fact]
        public void ShouldBeEqualByValue()
        {
            var mock1 = GetMock(1);
            var mock2 = GetMock(1);

            var id1 = mock1.Object;
            var id2 = mock2.Object;

            Assert.Equal(id1, id2);
        }

        [Fact]
        public void ShouldBeEqualByReference()
        {
            var id = GetMock(1).Object;

            Assert.Equal(id, id);
        }

        [Fact]
        public void ShouldNotBeEqualByValue()
        {
            var id1 = GetMock(1).Object;
            var id2 = GetMock(2).Object;

            Assert.NotEqual(id1, id2);
        }

        #region TypeEqualityTest

        [Fact]
        public void ShouldNotBeEqualByType()
        {
            var id1 = new Id1(1);
            var id2 = new Id2(1);

            Assert.True(id1 != id2);
        }

        internal class Id1 : IntId
        {
            public Id1(int id) : base(id)
            {
            }
        }

        internal class Id2 : IntId
        {
            public Id2(int id) : base(id)
            {
            }
        }

        #endregion

        [Fact]
        public void ShouldNotBeEqualBy0()
        {
            var id1 = GetMock(0).Object;
            var id2 = GetMock(0).Object;

            Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void IdPropertyShouldBeSet()
        {
            var id1 = GetMock(1).Object;

            Assert.Equal(1, id1.Id);
        }
    }
}
