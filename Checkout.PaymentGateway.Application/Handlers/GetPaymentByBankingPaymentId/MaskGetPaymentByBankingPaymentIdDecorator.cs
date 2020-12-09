using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers.GetPaymentByBankingPaymentId
{
    public class MaskGetPaymentByBankingPaymentIdDecorator : MaskDecorator<Domain.Queries.GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult>
    {
        public MaskGetPaymentByBankingPaymentIdDecorator(IQueryHandler<Domain.Queries.GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult> internalHandler) : base(internalHandler)
        {
        }

        protected override Task<GetPaymentByBankingPaymentIdResult> HandleDecoratorAsync(Domain.Queries.GetPaymentByBankingPaymentId query, GetPaymentByBankingPaymentIdResult result)
        {
            result.CardNumber = Mask(result.CardNumber, 4);
            return Task.FromResult(result);
        }
    }
}
