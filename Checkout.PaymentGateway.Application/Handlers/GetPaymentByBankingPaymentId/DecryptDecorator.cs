using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Queries;

namespace Checkout.PaymentGateway.Application.Handlers.GetPaymentByBankingPaymentId
{
    public sealed class DecryptDecorator : PostDecryptDecorator<Domain.Queries.GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult>
    {
        public DecryptDecorator(IQueryHandler<Domain.Queries.GetPaymentByBankingPaymentId, GetPaymentByBankingPaymentIdResult> internalHandler, IEncryptionService encryptionService) : base(internalHandler, encryptionService)
        {
        }

        protected override Task<GetPaymentByBankingPaymentIdResult> HandleDecoratorAsync(Domain.Queries.GetPaymentByBankingPaymentId query, GetPaymentByBankingPaymentIdResult result)
        {
            result.CardNumber = Decrypt(result.CardNumber);
            return Task.FromResult(result);
        }
    }
}
