using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Queries;
using Checkout.PaymentGateway.Domain.Repositories;

namespace Checkout.PaymentGateway.Application.Handlers.GetPaymentByBankingPaymentId
{
    [Mask(typeof(Domain.Queries.GetPaymentByBankingPaymentId), typeof(GetPaymentByBankingPaymentIdResult))]
    [Decrypt(typeof(Domain.Queries.GetPaymentByBankingPaymentId), typeof(GetPaymentByBankingPaymentIdResult), DecoratorExecutionTime.Post)]
    public class GetPaymentByBankingPaymentIdHandler : IQueryHandler<Domain.Queries.GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult>
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentByBankingPaymentIdHandler(IPaymentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<GetPaymentByBankingPaymentIdResult> HandleAsync(Domain.Queries.GetPaymentByBankingPaymentId query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));

            var aggregate = await _repository.GetByBankingPaymentIdAsync(query.BankingPaymentId);

            if (aggregate is null)
                return null;

            return new GetPaymentByBankingPaymentIdResult()
            {
                CardNumber = aggregate.CardNumber.Value,
                Amount = aggregate.Amount,
                Currency = aggregate.Currency.Value,
                SuccessfulPayment = aggregate.SuccessfulPayment
            };
        }
    }
}
