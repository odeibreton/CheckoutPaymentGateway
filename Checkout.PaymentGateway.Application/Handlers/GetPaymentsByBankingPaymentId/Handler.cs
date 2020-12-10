using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Repositories;

namespace Checkout.PaymentGateway.Application.Handlers.GetPaymentsByBankingPaymentId
{
    [Mask(typeof(Domain.Queries.GetPaymentsByBankingPaymentId), typeof(GetPaymentsByBankingPaymentIdResult))]
    [Decrypt(typeof(Domain.Queries.GetPaymentsByBankingPaymentId), typeof(GetPaymentsByBankingPaymentIdResult), Abstractions.DecoratorExecutionTime.Post)]
    public sealed class Handler : IQueryHandler<Domain.Queries.GetPaymentsByBankingPaymentId, GetPaymentsByBankingPaymentIdResult>
    {
        private readonly IPaymentRepository _repository;

        public Handler(IPaymentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<GetPaymentsByBankingPaymentIdResult> HandleAsync(Domain.Queries.GetPaymentsByBankingPaymentId query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));

            var aggregates = await _repository.GetPaymentsByBankingPaymentIdAsync(query.Ids);

            return new GetPaymentsByBankingPaymentIdResult()
            {
                Payments = aggregates.Select(p => new GetPaymentByBankingPaymentIdResult()
                {
                    CardNumber = p.CardNumber.Value,
                    Currency = p.Currency.Value,
                    Amount = p.Amount,
                    SuccessfulPayment = p.SuccessfulPayment
                }).ToList()
            };
        }
    }
}
