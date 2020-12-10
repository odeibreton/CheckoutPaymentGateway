using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers.GetPaymentsByBankingPaymentId
{
    public class MaskDecorator : MaskDecorator<Domain.Queries.GetPaymentsByBankingPaymentId, GetPaymentsByBankingPaymentIdResult>
    {
        public MaskDecorator(IQueryHandler<Domain.Queries.GetPaymentsByBankingPaymentId, GetPaymentsByBankingPaymentIdResult> internalHandler) : base(internalHandler)
        {
        }

        protected override Task<GetPaymentsByBankingPaymentIdResult> HandleDecoratorAsync(Domain.Queries.GetPaymentsByBankingPaymentId query, GetPaymentsByBankingPaymentIdResult result)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            result.Payments.ForEach(p => p.CardNumber = Mask(p.CardNumber, 4));

            return Task.FromResult(result);
        }
    }
}
