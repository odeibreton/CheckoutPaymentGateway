using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.DTO;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers.GetPaymentsByBankingPaymentId
{
    public class DecryptDecorator : PostDecryptDecorator<Domain.Queries.GetPaymentsByBankingPaymentId, GetPaymentsByBankingPaymentIdResult>
    {
        public DecryptDecorator(IQueryHandler<Domain.Queries.GetPaymentsByBankingPaymentId, GetPaymentsByBankingPaymentIdResult> internalHandler, IEncryptionService encryptionService) : base(internalHandler, encryptionService)
        {
        }

        protected override Task<GetPaymentsByBankingPaymentIdResult> HandleDecoratorAsync(Domain.Queries.GetPaymentsByBankingPaymentId query, GetPaymentsByBankingPaymentIdResult result)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            result.Payments.ForEach(p => p.CardNumber = Decrypt(p.CardNumber));

            return Task.FromResult(result);
        }
    }
}
