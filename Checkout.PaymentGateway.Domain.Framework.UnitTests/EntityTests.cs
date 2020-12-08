using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Domain.Framework.UnitTests
{
    public class EntityTests
    {
        private static Mock<Entity<int?>> GetMock(int? id)
        {
            return new Mock<Entity<int?>>(id)
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
            var entity1 = GetMock(1);
            var entity2 = GetMock(1);

            Assert.True(entity1 != entity2);
        }

        internal class Entity1 : Entity<int?>
        {
            public Entity1(int id) : base(id)
            {
            }
        }

        internal class Entity2 : Entity<int?>
        {
            public Entity2(int id) : base(id)
            {
            }
        }

        #endregion
    }
}
