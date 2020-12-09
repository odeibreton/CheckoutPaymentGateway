using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Domain.Framework;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class MaskDecoratorTests
    {
        #region Creation
        private static TestQueryHandler GetHandler() => new TestQueryHandler();
        private static TestMaskDecorator GetDecorator(IQueryHandler<TestQuery, TestQueryResult> handler) => new TestMaskDecorator(handler);
        #endregion

        [Theory]
        [InlineData("4485388069721426", 4, "************1426")]
        [InlineData("4485388069721426", 7, "*********9721426")]
        [InlineData("4485388069721426", 0, "****************")]
        public async void ShouldMaskString(string input, int visibleChars, string expected)
        {
            var handler = GetHandler();
            var decorator = GetDecorator(handler);

            var query = new TestQuery()
            {
                ToMask = input,
                VisibleCharacters = visibleChars
            };

            var result = await decorator.HandleAsync(query);

            Assert.Equal(expected, result.Masked);
        }

        #region Setup
        internal class TestQuery : IQuery
        {
            public string ToMask { get; set; }
            public int VisibleCharacters { get; set; }
        }

        internal class TestQueryResult
        {
            public string Masked { get; set; }
            public Queue<string> History { get; set; } = new Queue<string>();
        }

        internal class TestQueryHandler : IQueryHandler<TestQuery, TestQueryResult>
        {
            public Task<TestQueryResult> HandleAsync(TestQuery query)
            {
                var result = new TestQueryResult();
                result.History.Enqueue("Handler");
                return Task.FromResult(result);
            }
        }

        internal class TestMaskDecorator : MaskDecorator<TestQuery, TestQueryResult>
        {
            public TestMaskDecorator(IQueryHandler<TestQuery, TestQueryResult> internalHandler) : base(internalHandler)
            {
            }

            protected override Task<TestQueryResult> HandleDecoratorAsync(TestQuery query, TestQueryResult result)
            {
                result.Masked = Mask(query.ToMask, query.VisibleCharacters);
                result.History.Enqueue("Decorator");
                return Task.FromResult(result);
            }
        }
        #endregion
    }
}
