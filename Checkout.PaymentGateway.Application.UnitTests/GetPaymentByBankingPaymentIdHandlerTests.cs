using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Domain;
using Checkout.PaymentGateway.Domain.Queries;
using Checkout.PaymentGateway.Domain.Repositories;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Application.UnitTests
{
    public class GetPaymentByBankingPaymentIdHandlerTests
    {
        #region Creation
        private static GetPaymentByBankingPaymentIdHandler GetHandler(IPaymentRepository repository) =>
            new GetPaymentByBankingPaymentIdHandler(repository);

        private static Mock<IPaymentRepository> GetRepositoryMock() => new Mock<IPaymentRepository>();
        #endregion

        [Fact]
        public void ShouldReturnArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetPaymentByBankingPaymentIdHandler(null));
        }

        [Theory]
        [ClassData(typeof(ShouldReturnCorrectQueryResultData))]
        public async void ShouldReturnCorrectQueryResult(GetPaymentByBankingPaymentId query,
                                                         Payment payment,
                                                         GetPaymentByBankingPaymentIdResult expected)
        {
            var repositoryMock = GetRepositoryMock();
            repositoryMock.Setup(r => r.GetByBankingPaymentIdAsync(It.Is<string>(actual => actual == query.BankingPaymentId)))
                .ReturnsAsync(payment);

            var handler = GetHandler(repositoryMock.Object);

            var result = await handler.HandleAsync(query);

            Assert.NotNull(result);
            Assert.Equal(expected.CardNumber, result.CardNumber);
            Assert.Equal(expected.Currency, result.Currency);
            Assert.Equal(expected.Amount, result.Amount);
            Assert.Equal(expected.SuccessfulPayment, result.SuccessfulPayment);
        }

        internal class ShouldReturnCorrectQueryResultData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new GetPaymentByBankingPaymentId()
                    {
                        BankingPaymentId = "123"
                    },
                    new Payment(0, "123", true, new CardNumber("4080231619817071"), 12, 2020, new CVV("123"), 200m, new Currency("EUR")),
                    new GetPaymentByBankingPaymentIdResult()
                    {
                        CardNumber = "************7071",
                        Amount = 200m,
                        Currency = "EUR",
                        SuccessfulPayment = true
                    }
                };
                yield return new object[]
                {
                    new GetPaymentByBankingPaymentId()
                    {
                        BankingPaymentId = "456456"
                    },
                    new Payment(0, "456456", false, new CardNumber("4485478220890208503"), 12, 2020, new CVV("123"), 200m, new Currency("EUR")),
                    new GetPaymentByBankingPaymentIdResult()
                    {
                        CardNumber = "***************8503",
                        Amount = 200m,
                        Currency = "EUR",
                        SuccessfulPayment = false
                    }
                };
                yield return new object[]
                {
                    new GetPaymentByBankingPaymentId()
                    {
                        BankingPaymentId = "123"
                    },
                    new Payment(0, "123", false, new CardNumber("4080 2316 1981 7071"), 12, 2020, new CVV("123"), 200m, new Currency("EUR")),
                    new GetPaymentByBankingPaymentIdResult()
                    {
                        CardNumber = "************7071",
                        Amount = 200m,
                        Currency = "EUR",
                        SuccessfulPayment = false
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public async void ShouldReturnNullIfEntityIsNotFound()
        {
            var query = new GetPaymentByBankingPaymentId()
            {
                BankingPaymentId = "123"
            };

            var repositoryMock = GetRepositoryMock();
            repositoryMock.Setup(r => r.GetByBankingPaymentIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var handler = GetHandler(repositoryMock.Object);

            var result = await handler.HandleAsync(query);

            Assert.Null(result);
        }
    }
}
