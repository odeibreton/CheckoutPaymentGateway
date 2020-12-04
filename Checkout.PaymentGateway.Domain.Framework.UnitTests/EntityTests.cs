using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Domain.Framework.UnitTests
{
    public class EntityTests
    {
        private static Mock<IntId> GetIdMock(int id)
        {
            return new Mock<IntId>(id)
            {
                CallBase = true
            };
        }

        private static Mock<Entity<IntId>> GetMock(int? id)
        {
            IntId intId = null;

            if (id.HasValue)
            {
                intId = GetIdMock(id.Value).Object;
            }

            return new Mock<Entity<IntId>>(intId)
            {
                CallBase = true
            };
        }

        [Fact]
        public void ShouldBeEqualById()
        {
            var mock1 = GetMock(1);
            var mock2 = GetMock(1);

            var entity1 = mock1.Object;
            var entity2 = mock2.Object;

            Assert.Equal(entity1, entity2);
        }

        [Fact]
        public void ShouldBeEqualByReference()
        {
            var id = GetMock(1).Object;

            Assert.Equal(id, id);
        }

        [Fact]
        public void ShouldNotBeEqualById()
        {
            var entity1 = GetMock(1).Object;
            var entity2 = GetMock(2).Object;

            Assert.NotEqual(entity1, entity2);
        }

        [Fact]
        public void ShouldNotBeEqualByNullId()
        {
            var entity1 = GetMock(1).Object;
            var entity2 = GetMock(null).Object;

            Assert.NotEqual(entity1, entity2);
        }

        #region TypeEqualityTest
        [Fact]
        public void ShouldNotBeEqualByType()
        {
            var entity1 = GetIdMock(1);
            var entity2 = GetIdMock(1);

            Assert.True(entity1 != entity2);
        }

        internal class Entity1 : Entity<IntId>
        {
            public Entity1(IntId id) : base(id)
            {
            }
        }

        internal class Entity2 : Entity<IntId>
        {
            public Entity2(IntId id) : base(id)
            {
            }
        }

        #endregion
    }
}
