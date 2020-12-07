using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Infrastructure.UnitTests
{
    public class RepositoryTests
    {
        #region Creation
        private static TestRepository GetRepository(TestDbContext dbContext = null, ILogger<Repository> logger = null)
        {
            if (dbContext is null)
            {
                dbContext = GetContext(logger);
            }

            if (logger is null)
            {
                logger = NullLogger<Repository>.Instance;
            }

            return new TestRepository(dbContext, logger);
        }

        private static TestDbContext GetContext(ILogger<Repository> logger = null)
        {
            var options = new DbContextOptionsBuilder<PaymentGatewayDbContext>();
            options.UseInMemoryDatabase("Test")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            if (logger != null)
            {
                var factoryMock = new Mock<ILoggerFactory>();
                factoryMock.Setup(f => f.CreateLogger(It.IsAny<string>()))
                    .Returns(logger);
                options.UseLoggerFactory(factoryMock.Object);
            }

            return new TestDbContext(options.Options);
        }
        #endregion

        [Theory]
        [ClassData(typeof(ShouldThrowArgumentNullExceptionData))]
        public void ShouldThrowArgumentNullException(TestDbContext dbContext, ILogger<Repository> logger)
        {
            Assert.Throws<ArgumentNullException>(() => new TestRepository(dbContext, logger));
        }

        internal class ShouldThrowArgumentNullExceptionData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null,
                    NullLogger<TestRepository>.Instance
                };
                yield return new object[]
                {
                    new TestDbContext(),
                    null
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public async void ShouldSaveChanges()
        {
            var dbContext = GetContext();
            var sut = GetRepository(dbContext);
            var entity = new TestEntity()
            {
                Id = 1,
                Name = nameof(ShouldSaveChanges)
            };

            await sut.AddEntity(entity);
            await sut.SaveChangesAsync();

            Assert.NotNull(dbContext.Entities.Find(entity.Id));
            Assert.Equal(entity.Name, dbContext.Entities.Find(entity.Id).Name);
        }

        #region Setup
        internal class TestRepository : Repository
        {
            public TestRepository(TestDbContext dbContext, ILogger<Repository> logger) : base(dbContext, logger)
            {
            }

            public Task AddEntity(TestEntity entity)
            {
                return DbContext.AddAsync(entity).AsTask();
            }
        }

        internal class TestLogger<T> : ILogger<T>
        {
            private readonly List<EventId> _events = new List<EventId>();
            public IReadOnlyList<EventId> Events => _events;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _events.Add(eventId);
            }

            public bool IsEnabled(LogLevel logLevel) => true;

            public IDisposable BeginScope<TState>(TState state) => null;
        }

        public class TestDbContext : PaymentGatewayDbContext
        {
            public TestDbContext()
            {
            }

            public TestDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options)
            {
            }

            public DbSet<TestEntity> Entities { get; protected set; }
        }

        public class TestEntity
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Prop { get; set; } = "Value";
        }
        #endregion
    }
}
