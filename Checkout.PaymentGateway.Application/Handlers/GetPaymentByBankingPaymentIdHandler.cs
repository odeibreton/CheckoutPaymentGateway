using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Queries;
using Checkout.PaymentGateway.Domain.Repositories;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public class GetPaymentByBankingPaymentIdHandler : IQueryHandler<GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult>
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentByBankingPaymentIdHandler(IPaymentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<GetPaymentByBankingPaymentIdResult> HandleAsync(GetPaymentByBankingPaymentId query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));

            var aggregate = await _repository.GetByBankingPaymentIdAsync(query.BankingPaymentId);

            return new GetPaymentByBankingPaymentIdResult()
            {
                CardNumber = MaskCardNumber(aggregate.CardNumber.Value),
                Amount = aggregate.Amount,
                Currency = aggregate.Currency.Value,
                SuccessfulPayment = true
            };
        }

        private static string MaskCardNumber(string cardNumber)
        {
            var formattedNumber = cardNumber.Replace(" ", "");
            var last4digits = formattedNumber.Remove(0, formattedNumber.Length - 4);
            return new string('*', formattedNumber.Length - 4) + last4digits;
        }
    }
}
